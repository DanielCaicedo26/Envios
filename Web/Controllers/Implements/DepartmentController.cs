using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.DepartmentDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class DepartmentController : GenericController<DepartmentDto, Department>, IDepartmentController
    {
        private readonly IDepartmentBusiness _departmentBusiness;

        public DepartmentController(IDepartmentBusiness departmentBusiness, ILogger<DepartmentController> logger)
            : base(departmentBusiness, logger)
        {
            _departmentBusiness = departmentBusiness;
        }

        protected override int GetEntityId(DepartmentDto dto)
        {
            return dto.Id;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialDepartment([FromBody] UpdateDepartmentDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _departmentBusiness.UpdatePartialDepartmentAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente departamento: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente departamento: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("logic/{id}")]
        public async Task<IActionResult> DeleteLogicDepartment(int id)
        {
            try
            {
                var dto = new DeleteLogicalDepartmentDto { Id = id, Status = false };
                var result = await _departmentBusiness.DeleteLogicDepartmentAsync(dto);
                if (!result)
                    return NotFound($"Departamento con ID {id} no encontrado");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al eliminar lógicamente departamento: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar lógicamente departamento con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("country/{countryId}")]
        public async Task<IActionResult> GetByCountryId(int countryId)
        {
            try
            {
                var departments = await _departmentBusiness.GetByCountryIdAsync(countryId);
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener departamentos por país {countryId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var department = await _departmentBusiness.GetByNameAsync(name);
                if (department == null)
                    return NotFound($"Departamento con nombre {name} no encontrado");

                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener departamento por nombre {name}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}