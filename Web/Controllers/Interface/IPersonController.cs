using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.PersonDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface IPersonController : IGenericController<PersonDto, Person>
    {
        Task<IActionResult> UpdatePartialPerson(UpdatePersonDto dto);
        Task<IActionResult> DeleteLogicPerson(int id);
        Task<IActionResult> GetByDocument(string documentType, string documentNumber);
        Task<IActionResult> GetByPhone(string phoneNumber);
        Task<IActionResult> GetByLocation(int countryId, int? departmentId = null, int? cityId = null);
    }
}