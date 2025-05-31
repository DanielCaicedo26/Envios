using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.ProviderDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface IProviderController : IGenericController<ProviderDto, Provider>
    {
        Task<IActionResult> UpdatePartialProvider(UpdateProviderDto dto);
        Task<IActionResult> DeleteLogicProvider(int id);
        Task<IActionResult> GetByProviderCode(string providerCode);
        Task<IActionResult> GetByTaxId(string taxId);
        Task<IActionResult> GetByEmail(string email);
        Task<IActionResult> GetByLocation(int countryId, int? departmentId = null, int? cityId = null);
        Task<IActionResult> GetByBusinessType(string businessType);
    }
}