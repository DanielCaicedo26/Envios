using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.ProviderDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class ProviderBusiness : BaseBusiness<Provider, ProviderDto>, IProviderBusiness
    {
        private readonly IProviderData _providerData;

        public ProviderBusiness(IProviderData providerData, IMapper mapper, ILogger<ProviderBusiness> logger, IGenericIHelpers helpers)
            : base(providerData, mapper, logger, helpers)
        {
            _providerData = providerData;
        }

        public async Task<bool> UpdatePartialProviderAsync(UpdateProviderDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var provider = _mapper.Map<Provider>(dto);
            return await _providerData.UpdatePartial(provider);
        }

        public async Task<bool> DeleteLogicProviderAsync(DeleteLogicalProviderDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del proveedor es inválido");

            var exists = await _providerData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("provider", dto.Id);

            return await _providerData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<ProviderDto> GetByProviderCodeAsync(string providerCode)
        {
            var provider = await _providerData.GetByProviderCodeAsync(providerCode);
            return _mapper.Map<ProviderDto>(provider);
        }

        public async Task<ProviderDto> GetByTaxIdAsync(string taxId)
        {
            var provider = await _providerData.GetByTaxIdAsync(taxId);
            return _mapper.Map<ProviderDto>(provider);
        }

        public async Task<ProviderDto> GetByEmailAsync(string email)
        {
            var provider = await _providerData.GetByEmailAsync(email);
            return _mapper.Map<ProviderDto>(provider);
        }

        public async Task<List<ProviderDto>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null)
        {
            var providers = await _providerData.GetByLocationAsync(countryId, departmentId, cityId);
            return _mapper.Map<List<ProviderDto>>(providers);
        }

        public async Task<List<ProviderDto>> GetByBusinessTypeAsync(string businessType)
        {
            var providers = await _providerData.GetByBusinessTypeAsync(businessType);
            return _mapper.Map<List<ProviderDto>>(providers);
        }
    }
}