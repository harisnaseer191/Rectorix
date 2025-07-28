using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rectorix.Application.DTOs.Auth;
using Rectorix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<UserRoles> _roleManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<UserRoles> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<IdentityResult> RegisterAsync(UserRegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return result;

            if (!await _roleManager.RoleExistsAsync(dto.Role))
            {
                await _roleManager.CreateAsync(new UserRoles { Name = dto.Role });
            }

            await _userManager.AddToRoleAsync(user, dto.Role);
            return result;
        }

        public async Task<TokenDto?> LoginAsync(UserLoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return null;

            return await GenerateTokensAsync(user);
        }

        public async Task<TokenDto?> RefreshTokenAsync(string refreshToken)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return null;

            return await GenerateTokensAsync(user);
        }

        public async Task LogoutAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                await _userManager.UpdateAsync(user);
            }
        }

        private async Task<TokenDto> GenerateTokensAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName!)
        };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = Guid.NewGuid().ToString();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };
        }
    }

}
