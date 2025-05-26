using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.UserDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class UserController : GenericController<UserDto, User>, IUserController
    {
        private readonly IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness, ILogger<UserController> logger)
            : base(userBusiness, logger)
        {
            _userBusiness = userBusiness;
        }

        protected override int GetEntityId(UserDto dto)
        {
            return dto.Id;
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userBusiness.GetByEmailAsync(email);
                if (user == null)
                    return NotFound($"Usuario con email {email} no encontrado");

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener usuario con email {email}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialUser([FromBody] UpdateUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userBusiness.UpdateParcialUserAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente usuario: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente usuario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }



        //Este metodo responde a patch /users//{id}/status
        [HttpPatch("users/{id}/status")]
        public async Task<IActionResult> SetUserActive(int id, [FromBody] UserStatusDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                // Ahora pasamos el id recibido como parámetro
                var deleteLogicalUserDto = new DeleteLogicalUserDto
                {
                    Id = id,
                    Status = dto.IsActive
                };

                var result = await _userBusiness.SetUserActiveAsync(deleteLogicalUserDto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al cambiar estado de usuario: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al cambiar estado de usuario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }




           }



}