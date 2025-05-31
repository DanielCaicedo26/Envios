using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.CountryDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface ICountryController : IGenericController<CountryDto, Country>
    {
        Task<IActionResult> UpdatePartialCountry(UpdateCountryDto dto);
        Task<IActionResult> DeleteLogicCountry(int id);
        Task<IActionResult> GetByName(string name);
    }
}