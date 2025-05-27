using Business.Interfaces;
using Entity.Dtos.AuthDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Utilities.Interfaces;

namespace Business.Services
{
    public class JwtService : IJwtService
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly ILogger<JwtService> _logger;

        public JwtService(IJwtGenerator jwtGenerator, ILogger<JwtService> logger)
        {
            _jwtGenerator = jwtGenerator;
            _logger = logger;
        }

        public async Task<AuthDto> GenerateTokenAsync(User user)
        {
            try
            {
                var token = await _jwtGenerator.GeneradorToken(user);
                _logger.LogInformation($"Token generado exitosamente para usuario ID: {user.Id}");
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al generar token para usuario ID {user.Id}: {ex.Message}");
                throw;
            }
        }

        public string GenerateRecoveryToken(User user, int expirationMinutes = 15)
        {
            try
            {
                var token = _jwtGenerator.GenerarTokenRecuperacion(user, expirationMinutes);
                _logger.LogInformation($"Token de recuperación generado para usuario ID: {user.Id}");
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al generar token de recuperación para usuario ID {user.Id}: {ex.Message}");
                throw;
            }
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var claimsPrincipal = _jwtGenerator.ValidateToken(token);
                if (claimsPrincipal != null)
                {
                    _logger.LogInformation("Token validado exitosamente");
                }
                else
                {
                    _logger.LogWarning("Token inválido o expirado");
                }
                return claimsPrincipal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al validar token: {ex.Message}");
                return null;
            }
        }
    }
}