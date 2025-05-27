using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.ClientDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class ClientBusiness : BaseBusiness<Client, ClientDto>, IClientBusiness
    {
        private readonly IClientData _clientData;

        public ClientBusiness(IClientData clientData, IMapper mapper, ILogger<ClientBusiness> logger, IGenericIHelpers helpers)
            : base(clientData, mapper, logger, helpers)
        {
            _clientData = clientData;
        }

        public async Task<bool> UpdatePartialClientAsync(UpdateClientDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var client = _mapper.Map<Client>(dto);
            return await _clientData.UpdatePartial(client);
        }

        public async Task<bool> DeleteLogicClientAsync(DeleteLogicalClientDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del cliente es inválido");

            var exists = await _clientData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("client", dto.Id);

            return await _clientData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<ClientDto> GetByClientCodeAsync(string clientCode)
        {
            var client = await _clientData.GetByClientCodeAsync(clientCode);
            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> GetByTaxIdAsync(string taxId)
        {
            var client = await _clientData.GetByTaxIdAsync(taxId);
            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> GetByEmailAsync(string email)
        {
            var client = await _clientData.GetByEmailAsync(email);
            return _mapper.Map<ClientDto>(client);
        }

        public async Task<List<ClientDto>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null)
        {
            var clients = await _clientData.GetByLocationAsync(countryId, departmentId, cityId);
            return _mapper.Map<List<ClientDto>>(clients);
        }

        public async Task<List<ClientDto>> GetByCreditLimitRangeAsync(decimal minLimit, decimal maxLimit)
        {
            var clients = await _clientData.GetByCreditLimitRangeAsync(minLimit, maxLimit);
            return _mapper.Map<List<ClientDto>>(clients);
        }
    }
}