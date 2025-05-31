using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.ModuleDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class ModuleController : GenericController<ModuleDto, Module>, IModuleController
    {
        private readonly IModuleBusiness _moduleBusiness;

        public ModuleController(IModuleBusiness moduleBusiness, ILogger<ModuleController> logger)
            : base(moduleBusiness, logger)
        {
            _moduleBusiness = moduleBusiness;
        }

        protected override int GetEntityId(ModuleDto dto)
        {
            return dto.Id;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialModule([FromBody] UpdateModuleDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _moduleBusiness.UpdatePartialModuleAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente módulo: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente módulo: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("logic/{id}")]
        public async Task<IActionResult> DeleteLogicModule(int id)
        {
            try
            {
                var dto = new DeleteLogicalModuleDto { Id = id, Status = false };
                var result = await _moduleBusiness.DeleteLogicModuleAsync(dto);
                if (!result)
                    return NotFound($"Módulo con ID {id} no encontrado");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al eliminar lógicamente módulo: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar lógicamente módulo con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var module = await _moduleBusiness.GetByNameAsync(name);
                if (module == null)
                    return NotFound($"Módulo con nombre {name} no encontrado");

                return Ok(module);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener módulo por nombre {name}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("form/{formId}")]
        public async Task<IActionResult> GetModulesByFormId(int formId)
        {
            try
            {
                var modules = await _moduleBusiness.GetModulesByFormIdAsync(formId);
                return Ok(modules);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener módulos por formulario {formId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchModules([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return BadRequest("El término de búsqueda es requerido");

                var modules = await _moduleBusiness.SearchModulesAsync(searchTerm);
                return Ok(modules);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al buscar módulos con término {searchTerm}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
