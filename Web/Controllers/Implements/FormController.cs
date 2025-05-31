using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.FormDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class FormController : GenericController<FormDto, Form>, IFormController
    {
        private readonly IFormBusiness _formBusiness;

        public FormController(IFormBusiness formBusiness, ILogger<FormController> logger)
            : base(formBusiness, logger)
        {
            _formBusiness = formBusiness;
        }

        protected override int GetEntityId(FormDto dto)
        {
            return dto.Id;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialForm([FromBody] UpdateFormDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _formBusiness.UpdatePartialFormAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente formulario: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente formulario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("logic/{id}")]
        public async Task<IActionResult> DeleteLogicForm(int id)
        {
            try
            {
                var dto = new DeleteLogicalFormDto { Id = id, Status = false };
                var result = await _formBusiness.DeleteLogicFormAsync(dto);
                if (!result)
                    return NotFound($"Formulario con ID {id} no encontrado");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al eliminar lógicamente formulario: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar lógicamente formulario con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var form = await _formBusiness.GetByNameAsync(name);
                if (form == null)
                    return NotFound($"Formulario con nombre {name} no encontrado");

                return Ok(form);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener formulario por nombre {name}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("module/{moduleId}")]
        public async Task<IActionResult> GetFormsByModuleId(int moduleId)
        {
            try
            {
                var forms = await _formBusiness.GetFormsByModuleIdAsync(moduleId);
                return Ok(forms);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener formularios por módulo {moduleId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchForms([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return BadRequest("El término de búsqueda es requerido");

                var forms = await _formBusiness.SearchFormsAsync(searchTerm);
                return Ok(forms);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al buscar formularios con término {searchTerm}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}