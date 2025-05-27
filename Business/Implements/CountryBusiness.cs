using AutoMapper;
using Business.Interfaces;
using Business.Interfaces.Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.CountryDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class CountryBusiness : BaseBusiness<Country, CountryDto>, ICountryBusiness
    {
        private readonly ICountryData _countryData;

        public CountryBusiness(ICountryData countryData, IMapper mapper, ILogger<CountryBusiness> logger, IGenericIHelpers helpers)
            : base(countryData, mapper, logger, helpers)
        {
            _countryData = countryData;
        }

        public async Task<bool> UpdatePartialCountryAsync(UpdateCountryDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var country = _mapper.Map<Country>(dto);
            return await _countryData.UpdatePartial(country);
        }

        public async Task<bool> DeleteLogicCountryAsync(DeleteLogicalCountryDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del país es inválido");

            var exists = await _countryData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("country", dto.Id);

            return await _countryData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<CountryDto> GetByNameAsync(string name)
        {
            var country = await _countryData.GetByNameAsync(name);
            return _mapper.Map<CountryDto>(country);
        }
    }
}