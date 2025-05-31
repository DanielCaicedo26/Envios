using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.ClientDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface IClientController : IGenericController<ClientDto, Client>
    {
        Task<IActionResult> UpdatePartialClient(UpdateClientDto dto);
        Task<IActionResult> DeleteLogicClient(int id);
        Task<IActionResult> GetByClientCode(string clientCode);
        Task<IActionResult> GetByTaxId(string taxId);
        Task<IActionResult> GetByEmail(string email);
        Task<IActionResult> GetByLocation(int countryId, int? departmentId = null, int? cityId = null);
        Task<IActionResult> GetByCreditLimitRange(decimal minLimit, decimal maxLimit);
    }
}