using Entity.Dtos.ClientDTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IClientBusiness : IBaseBusiness<Client, ClientDto>
    {
        Task<bool> UpdatePartialClientAsync(UpdateClientDto dto);
        Task<bool> DeleteLogicClientAsync(DeleteLogicalClientDto dto);
        Task<ClientDto> GetByClientCodeAsync(string clientCode);
        Task<ClientDto> GetByTaxIdAsync(string taxId);
        Task<ClientDto> GetByEmailAsync(string email);
        Task<List<ClientDto>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null);
        Task<List<ClientDto>> GetByCreditLimitRangeAsync(decimal minLimit, decimal maxLimit);
    }
}
