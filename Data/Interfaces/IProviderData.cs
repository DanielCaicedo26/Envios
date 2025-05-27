using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IProviderData : IBaseModelData<Provider>
    {
        Task<bool> ActiveAsync(int id, bool status);
        Task<bool> UpdatePartial(Provider provider);
        Task<Provider> GetByProviderCodeAsync(string providerCode);
        Task<Provider> GetByTaxIdAsync(string taxId);
        Task<Provider> GetByEmailAsync(string email);
        Task<List<Provider>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null);
        Task<List<Provider>> GetByBusinessTypeAsync(string businessType);
    }
}
