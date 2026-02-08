using System.Security.Claims;
using TodoListAPI.Models.Domain;

namespace TodoListAPI.JwtService
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user);
        RefreshToken CreateRefreshToken(Guid userId, TimeSpan validity, string? device = null, string? ip = null, string? ua = null);
        ClaimsPrincipal? ValidatePrincipalFromExpiredToken(string token);
    }
}

