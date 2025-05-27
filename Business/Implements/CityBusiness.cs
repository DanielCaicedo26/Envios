using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.CityDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class CityBusiness : BaseBusiness<City, CityDto>, ICityBusiness
    {
        private readonly ICityData _cityData;

        public CityBusiness(ICityData cityData, IMapper mapper, ILogger<CityBusiness> logger, IGenericIHelpers helpers)
            : base(cityData, mapper, logger, helpers)
        {
            _cityData = cityData;
        }

        public async Task<bool> UpdatePartialCityAsync(UpdateCityDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var city = _mapper.Map<City>(dto);
            return await _cityData.UpdatePartial(city);
        }

        public async Task<bool> DeleteLogicCityAsync(DeleteLogicalCityDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID de la ciudad es inválido");

            var exists = await _cityData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("city", dto.Id);

            return await _cityData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<List<CityDto>> GetByDepartmentIdAsync(int departmentId)
        {
            var cities = await _cityData.GetByDepartmentIdAsync(departmentId);
            return _mapper.Map<List<CityDto>>(cities);
        }

        public async Task<CityDto> GetByNameAsync(string name)
        {
            var city = await _cityData.GetByNameAsync(name);
            return _mapper.Map<CityDto>(city);
        }

        public async Task<CityDto> GetByPostalCodeAsync(string postalCode)
        {
            var city = await _cityData.GetByPostalCodeAsync(postalCode);
            return _mapper.Map<CityDto>(city);
        }
    }
}