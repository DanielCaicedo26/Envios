using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.PermissionDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class PermissionController : GenericController<PermissionDto, Permission>, IPermissionController
    {
        private readonly IPermissionBusiness _permissionBusiness;

        public PermissionController(IPermissionBusiness permissionBusiness, ILogger<PermissionController> logger)
            : base(permissionBusiness, logger)
        {
            _permissionBusiness = permissionBusiness;
        }

        protected override int GetEntityId(PermissionDto dto)
        {
            return dto.Id;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialPermission([FromBody] UpdatePermissionDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _permissionBusiness.UpdatePartialPermissionAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente permiso: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente permiso: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("logic/{id}")]
        public async Task<IActionResult> DeleteLogicPermission(int id)
        {
            try
            {
                var dto = new DeleteLogicalPermissionDto { Id = id, Status = false };
                var result = await _permissionBusiness.DeleteLogicPermissionAsync(dto);
                if (!result)
                    return NotFound($"Permiso con ID {id} no encontrado");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al eliminar lógicamente permiso: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar lógicamente permiso con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var permission = await _permissionBusiness.GetByNameAsync(name);
                if (permission == null)
                    return NotFound($"Permiso con nombre {name} no encontrado");

                return Ok(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener permiso por nombre {name}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetPermissionsByRoleId(int roleId)
        {
            try
            {
                var permissions = await _permissionBusiness.GetPermissionsByRoleIdAsync(roleId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener permisos por rol {roleId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPermissions([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return BadRequest("El término de búsqueda es requerido");

                var permissions = await _permissionBusiness.SearchPermissionsAsync(searchTerm);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al buscar permisos con término {searchTerm}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("basic")]
        public async Task<IActionResult> GetBasicPermissions()
        {
            try
            {
                var permissions = await _permissionBusiness.GetBasicPermissionsAsync();
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener permisos básicos: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}