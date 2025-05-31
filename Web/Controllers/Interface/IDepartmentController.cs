using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.DepartmentDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface IDepartmentController : IGenericController<DepartmentDto, Department>
    {
        Task<IActionResult> UpdatePartialDepartment(UpdateDepartmentDto dto);
        Task<IActionResult> DeleteLogicDepartment(int id);
        Task<IActionResult> GetByCountryId(int countryId);
        Task<IActionResult> GetByName(string name);
    }
}