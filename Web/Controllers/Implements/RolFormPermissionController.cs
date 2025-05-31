using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.RolFormPermissionDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class RolFormPermissionController : GenericController<RolFormPermissionDto, RolFormPermission>, IRolFormPermissionController
    {
        private readonly IRolFormPermissionBusiness _rolFormPermissionBusiness;

        public RolFormPermissionController(IRolFormPermissionBusiness rolFormPermissionBusiness, ILogger<RolFormPermissionController> logger)
            : base(rolFormPermissionBusiness, logger)
        {
            _rolFormPermissionBusiness = rolFormPermissionBusiness;
        }

        protected override int GetEntityId(RolFormPermissionDto dto)
        {
            return dto.Id;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialRolFormPermission([FromBody] UpdateRolFormPermissionDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _rolFormPermissionBusiness.UpdatePartialRolFormPermissionAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente permiso rol-formulario: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente permiso rol-formulario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("logic/{id}")]
        public async Task<IActionResult> DeleteLogicRolFormPermission(int id)
        {
            try
            {
                var dto = new DeleteLogicalRolFormPermissionDto { Id = id, Status = false };
                var result = await _rolFormPermissionBusiness.DeleteLogicRolFormPermissionAsync(dto);
                if (!result)
                    return NotFound($"Permiso rol-formulario con ID {id} no encontrado");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al eliminar lógicamente permiso rol-formulario: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar lógicamente permiso rol-formulario con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("role/{roleId}/form/{formId}/permission/{permissionId}")]
        public async Task<IActionResult> GetByRoleFormPermission(int roleId, int formId, int permissionId)
        {
            try
            {
                var rolFormPermission = await _rolFormPermissionBusiness.GetByRoleFormPermissionAsync(roleId, formId, permissionId);
                if (rolFormPermission == null)
                    return NotFound("Permiso rol-formulario no encontrado");

                return Ok(rolFormPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener permiso rol-formulario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetByRoleId(int roleId)
        {
            try
            {
                var permissions = await _rolFormPermissionBusiness.GetByRoleIdAsync(roleId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener permisos por rol {roleId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("form/{formId}")]
        public async Task<IActionResult> GetByFormId(int formId)
        {
            try
            {
                var permissions = await _rolFormPermissionBusiness.GetByFormIdAsync(formId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener permisos por formulario {formId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("permission/{permissionId}")]
        public async Task<IActionResult> GetByPermissionId(int permissionId)
        {
            try
            {
                var permissions = await _rolFormPermissionBusiness.GetByPermissionIdAsync(permissionId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener roles por permiso {permissionId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignPermissionToRoleForm(
            [FromQuery] int roleId,
            [FromQuery] int formId,
            [FromQuery] int permissionId,
            [FromQuery] bool canCreate = false,
            [FromQuery] bool canRead = false,
            [FromQuery] bool canUpdate = false,
            [FromQuery] bool canDelete = false)
        {
            try
            {
                var result = await _rolFormPermissionBusiness.AssignPermissionToRoleFormAsync(
                    roleId, formId, permissionId, canCreate, canRead, canUpdate, canDelete);

                if (!result)
                    return BadRequest("No se pudo asignar el permiso");

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al asignar permiso: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("crud-permissions")]
        public async Task<IActionResult> UpdateCRUDPermissions(
            [FromQuery] int roleId,
            [FromQuery] int formId,
            [FromQuery] int permissionId,
            [FromQuery] bool canCreate,
            [FromQuery] bool canRead,
            [FromQuery] bool canUpdate,
            [FromQuery] bool canDelete)
        {
            try
            {
                var result = await _rolFormPermissionBusiness.UpdateCRUDPermissionsAsync(
                    roleId, formId, permissionId, canCreate, canRead, canUpdate, canDelete);

                if (!result)
                    return NotFound("Permiso no encontrado");

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar permisos CRUD: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemovePermissionFromRoleForm(
            [FromQuery] int roleId,
            [FromQuery] int formId,
            [FromQuery] int permissionId)
        {
            try
            {
                var result = await _rolFormPermissionBusiness.RemovePermissionFromRoleFormAsync(roleId, formId, permissionId);
                if (!result)
                    return NotFound("Permiso no encontrado");

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al remover permiso: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("has-permission")]
        public async Task<IActionResult> HasPermission(
            [FromQuery] int roleId,
            [FromQuery] int formId,
            [FromQuery] string permissionType)
        {
            try
            {
                var hasPermission = await _rolFormPermissionBusiness.HasPermissionAsync(roleId, formId, permissionType);
                return Ok(new { HasPermission = hasPermission });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al verificar permiso: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
