using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.RolUserDTO;
using Entity.Model;
using Business.Interfaces;
using Web.Controllers.Interface;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class RoleUserController : GenericController<RolUserDto, RolUser>, IRoleUserController
    {
        private readonly IRoleUserBusiness _roleUserBusiness;

        public RoleUserController(IRoleUserBusiness roleUserBusiness, ILogger<RoleUserController> logger)
            : base(roleUserBusiness, logger)
        {
            _roleUserBusiness = roleUserBusiness;
        }

        protected override int GetEntityId(RolUserDto dto)
        {
            return dto.Id;
        }

        [HttpPatch("{id}/role/{roleId}")]
        public async Task<IActionResult> UpdatePartialRoleUser(int id, int roleId,UpdateRolUserDto dto)
        {
            try
            {
                var result = await _roleUserBusiness.UpdateParcialRoleUserAsync(dto);
                if (!result)
                    return NotFound($"Asignación de rol con ID {id} no encontrada");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente asignación de rol: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente asignación de rol: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}