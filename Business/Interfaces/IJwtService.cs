// Business/Interfaces/IJwtService.cs
using System.Security.Claims;
using System.Threading.Tasks;
using Entity.Dtos.AuthDTO;

using Entity.Model;

namespace Business.Interfaces
{
    /// <summary>
    /// Servicio para operaciones relacionadas con JWT en la capa de negocio.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>-+
        /// Genera un token JWT para un usuario autenticado.
        /// </summary>
        /// <param name="user">Usuario autenticado</param>
        /// <returns>DTO con el token generado y su fecha de expiración</returns>
        Task<AuthDto> GenerateTokenAsync(User user);

        /// <summary>
        /// Valida un token JWT y extrae sus claims.
        /// </summary>
        /// <param name="token">Token JWT a validar</param>
        /// <returns>ClaimsPrincipal con la información del token, o null si el token es inválido</returns>
        ClaimsPrincipal ValidateToken(string token);

        /// <summary>
        /// Verifica si un token es válido sin extraer sus claims.
        /// </summary>
        /// <param name="token">Token JWT a verificar</param>
        /// <returns>True si el token es válido; false en caso contrario</returns>
        bool IsTokenValid(string token);
    }
}