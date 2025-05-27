using Entity.Dtos.AuthDTO;
using Entity.Model;
using System.Security.Claims;

namespace Business.Interfaces
{
    public interface IJwtService
    {
        Task<AuthDto> GenerateTokenAsync(User user);
        string GenerateRecoveryToken(User user, int expirationMinutes = 15);
        ClaimsPrincipal ValidateToken(string token);
    }
}