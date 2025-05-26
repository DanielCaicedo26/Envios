using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IClientData : IBaseModelData<Client>
    {
        Task<bool> ActiveAsync(int id, bool status);
        Task<bool> UpdatePartial(Client client);
        Task<Client> GetByClientCodeAsync(string clientCode);
        Task<Client> GetByTaxIdAsync(string taxId);
        Task<Client> GetByEmailAsync(string email);
        Task<List<Client>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null);
        Task<List<Client>> GetByCreditLimitRangeAsync(decimal minLimit, decimal maxLimit);
    }
}
