using Entity.Dtos.AuthDTO;
using Entity.Dtos.CredencialesDTO;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> LoginAsync(CredencialesDto credenciales);
        Task<bool> ValidateCredentialsAsync(string email, string password);
    }
}