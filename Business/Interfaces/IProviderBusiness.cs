using Entity.Dtos.ProviderDTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProviderBusiness : IBaseBusiness<Provider, ProviderDto>
    {
        Task<bool> UpdatePartialProviderAsync(UpdateProviderDto dto);
        Task<bool> DeleteLogicProviderAsync(DeleteLogicalProviderDto dto);
        Task<ProviderDto> GetByProviderCodeAsync(string providerCode);
        Task<ProviderDto> GetByTaxIdAsync(string taxId);
        Task<ProviderDto> GetByEmailAsync(string email);
        Task<List<ProviderDto>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null);
        Task<List<ProviderDto>> GetByBusinessTypeAsync(string businessType);
    }
}
