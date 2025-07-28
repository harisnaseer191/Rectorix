using Microsoft.AspNetCore.Identity;
using Rectorix.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(UserRegisterDto dto);
        Task<TokenDto?> LoginAsync(UserLoginDto dto);
        Task<TokenDto?> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(Guid userId);
    }
}
