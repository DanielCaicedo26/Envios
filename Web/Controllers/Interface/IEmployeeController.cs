using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.EmployeeDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface IEmployeeController : IGenericController<EmployeeDto, Employee>
    {
        Task<IActionResult> UpdatePartialEmployee(UpdateEmployeeDto dto);
        Task<IActionResult> DeleteLogicEmployee(int id);
        Task<IActionResult> GetByEmployeeCode(string employeeCode);
        Task<IActionResult> GetBySupervisorId(int supervisorId);
        Task<IActionResult> GetActiveEmployees();
        Task<IActionResult> GetByLocation(int countryId, int? departmentId = null, int? cityId = null);
        Task<IActionResult> AssignSupervisor(int employeeId, int supervisorId);
    }
}