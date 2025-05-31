using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.ModuleFormDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class ModuleFormController : GenericController<ModuleFormDto, ModuleForm>, IModuleFormController
    {
        private readonly IModuleFormBusiness _moduleFormBusiness;

        public ModuleFormController(IModuleFormBusiness moduleFormBusiness, ILogger<ModuleFormController> logger)
            : base(moduleFormBusiness, logger)
        {
            _moduleFormBusiness = moduleFormBusiness;
        }

        protected override int GetEntityId(ModuleFormDto dto)
        {
            return dto.Id;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialModuleForm([FromBody] UpdateModuleFormDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _moduleFormBusiness.UpdatePartialModuleFormAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente módulo-formulario: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente módulo-formulario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("logic/{id}")]
        public async Task<IActionResult> DeleteLogicModuleForm(int id)
        {
            try
            {
                var dto = new DeleteLogicalModuleFormDto { Id = id, Status = false };
                var result = await _moduleFormBusiness.DeleteLogicModuleFormAsync(dto);
                if (!result)
                    return NotFound($"Relación módulo-formulario con ID {id} no encontrada");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al eliminar lógicamente módulo-formulario: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar lógicamente módulo-formulario con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("module/{moduleId}/form/{formId}")]
        public async Task<IActionResult> GetByModuleAndForm(int moduleId, int formId)
        {
            try
            {
                var moduleForm = await _moduleFormBusiness.GetByModuleAndFormAsync(moduleId, formId);
                if (moduleForm == null)
                    return NotFound($"Relación entre módulo {moduleId} y formulario {formId} no encontrada");

                return Ok(moduleForm);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener relación módulo-formulario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("module/{moduleId}")]
        public async Task<IActionResult> GetByModuleId(int moduleId)
        {
            try
            {
                var moduleForms = await _moduleFormBusiness.GetByModuleIdAsync(moduleId);
                return Ok(moduleForms);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener formularios por módulo {moduleId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("form/{formId}")]
        public async Task<IActionResult> GetByFormId(int formId)
        {
            try
            {
                var moduleForms = await _moduleFormBusiness.GetByFormIdAsync(formId);
                return Ok(moduleForms);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener módulos por formulario {formId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignFormToModule([FromQuery] int moduleId, [FromQuery] int formId)
        {
            try
            {
                var result = await _moduleFormBusiness.AssignFormToModuleAsync(moduleId, formId);
                if (!result)
                    return BadRequest("No se pudo asignar el formulario al módulo");

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al asignar formulario {formId} a módulo {moduleId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFormFromModule([FromQuery] int moduleId, [FromQuery] int formId)
        {
            try
            {
                var result = await _moduleFormBusiness.RemoveFormFromModuleAsync(moduleId, formId);
                if (!result)
                    return NotFound("Relación no encontrada");

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al remover formulario {formId} del módulo {moduleId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
