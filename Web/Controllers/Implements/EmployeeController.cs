using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.EmployeeDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class EmployeeController : GenericController<EmployeeDto, Employee>, IEmployeeController
    {
        private readonly IEmployeeBusiness _employeeBusiness;

        public EmployeeController(IEmployeeBusiness employeeBusiness, ILogger<EmployeeController> logger)
            : base(employeeBusiness, logger)
        {
            _employeeBusiness = employeeBusiness;
        }

        protected override int GetEntityId(EmployeeDto dto)
        {
            return dto.Id;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialEmployee([FromBody] UpdateEmployeeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _employeeBusiness.UpdatePartialEmployeeAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente empleado: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente empleado: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("logic/{id}")]
        public async Task<IActionResult> DeleteLogicEmployee(int id)
        {
            try
            {
                var dto = new DeleteLogicalEmployeeDto { Id = id, Status = false };
                var result = await _employeeBusiness.DeleteLogicEmployeeAsync(dto);
                if (!result)
                    return NotFound($"Empleado con ID {id} no encontrado");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al eliminar lógicamente empleado: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar lógicamente empleado con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("employee-code/{employeeCode}")]
        public async Task<IActionResult> GetByEmployeeCode(string employeeCode)
        {
            try
            {
                var employee = await _employeeBusiness.GetByEmployeeCodeAsync(employeeCode);
                if (employee == null)
                    return NotFound($"Empleado con código {employeeCode} no encontrado");

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener empleado por código {employeeCode}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("supervisor/{supervisorId}")]
        public async Task<IActionResult> GetBySupervisorId(int supervisorId)
        {
            try
            {
                var employees = await _employeeBusiness.GetBySupervisorIdAsync(supervisorId);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener empleados por supervisor {supervisorId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveEmployees()
        {
            try
            {
                var employees = await _employeeBusiness.GetActiveEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener empleados activos: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("location")]
        public async Task<IActionResult> GetByLocation([FromQuery] int countryId, [FromQuery] int? departmentId = null, [FromQuery] int? cityId = null)
        {
            try
            {
                var employees = await _employeeBusiness.GetByLocationAsync(countryId, departmentId, cityId);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener empleados por ubicación: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("{employeeId}/assign-supervisor/{supervisorId}")]
        public async Task<IActionResult> AssignSupervisor(int employeeId, int supervisorId)
        {
            try
            {
                var result = await _employeeBusiness.AssignSupervisorAsync(employeeId, supervisorId);
                if (!result)
                    return BadRequest("No se pudo asignar el supervisor");

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al asignar supervisor {supervisorId} a empleado {employeeId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}