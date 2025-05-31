using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.CityDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface ICityController : IGenericController<CityDto, City>
    {
        Task<IActionResult> UpdatePartialCity(UpdateCityDto dto);
        Task<IActionResult> DeleteLogicCity(int id);
        Task<IActionResult> GetByDepartmentId(int departmentId);
        Task<IActionResult> GetByName(string name);
        Task<IActionResult> GetByPostalCode(string postalCode);
    }
}