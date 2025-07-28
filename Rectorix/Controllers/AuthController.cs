using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rectorix.Application.DTOs.Auth;
using Rectorix.Application.Services.Auth;
using System.Security.Claims;

namespace Rectorix.API.Controllers
{

    public class AuthController : RectorixBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return result.Succeeded
                ? Ok(result)
                : BadRequest(result.Errors);
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var tokens = await _authService.LoginAsync(dto);
            return tokens == null
                ? Unauthorized("Invalid credentials")
                : Ok(tokens);
        }

        /// <summary>
        /// Refresh JWT token using refresh token
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto dto)
        {
            var tokens = await _authService.RefreshTokenAsync(dto.RefreshToken);
            return tokens == null
                ? Unauthorized("Invalid or expired refresh token")
                : Ok(tokens);
        }

        /// <summary>
        /// Logout the user (invalidate refresh token)
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            await _authService.LogoutAsync(Guid.Parse(userId));
            return Ok(new { Message = "Logged out successfully" });
        }
    }
}
