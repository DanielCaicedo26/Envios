using Business.Interfaces;
using Entity.Dtos.AuthDTO;
using Entity.Dtos.CredencialesDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Interfaces;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserBusiness userBusiness, IJwtGenerator jwtGenerator, ILogger<AuthService> logger)
        {
            _userBusiness = userBusiness;
            _jwtGenerator = jwtGenerator;
            _logger = logger;
        }

        public async Task<AuthDto> LoginAsync(CredencialesDto credenciales)
        {
            // Validar credenciales
            var user = await _userBusiness.LoginAsync(credenciales.Email, credenciales.Password);

            if (user == null)
            {
                _logger.LogWarning($"Intento de login fallido para {credenciales.Email}");
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            // Generar token JWT
            var token = await _jwtGenerator.GeneradorToken(user);

            _logger.LogInformation($"Login exitoso para {credenciales.Email}");
            return token;
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            return await _userBusiness.ValidateCredentialsAsync(email, password);
        }
    }
}