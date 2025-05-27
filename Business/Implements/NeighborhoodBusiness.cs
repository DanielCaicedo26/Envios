using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.NeighborhoodDTO;
using Entity.Model.Base;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class NeighborhoodBusiness : BaseBusiness<Neighborhood, NeighborhoodDto>, INeighborhoodBusiness
    {
        private readonly INeighborhoodData _neighborhoodData;

        public NeighborhoodBusiness(INeighborhoodData neighborhoodData, IMapper mapper, ILogger<NeighborhoodBusiness> logger, IGenericIHelpers helpers)
            : base(neighborhoodData, mapper, logger, helpers)
        {
            _neighborhoodData = neighborhoodData;
        }

        public async Task<bool> UpdatePartialNeighborhoodAsync(UpdateNeighborhoodDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var neighborhood = _mapper.Map<Neighborhood>(dto);
            return await _neighborhoodData.UpdatePartial(neighborhood);
        }

        public async Task<bool> DeleteLogicNeighborhoodAsync(DeleteLogicalNeighborhoodDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del barrio es inválido");

            var exists = await _neighborhoodData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("neighborhood", dto.Id);

            return await _neighborhoodData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<List<NeighborhoodDto>> GetByCityIdAsync(int cityId)
        {
            var neighborhoods = await _neighborhoodData.GetByCityIdAsync(cityId);
            return _mapper.Map<List<NeighborhoodDto>>(neighborhoods);
        }

        public async Task<NeighborhoodDto> GetByNameAsync(string name)
        {
            var neighborhood = await _neighborhoodData.GetByNameAsync(name);
            return _mapper.Map<NeighborhoodDto>(neighborhood);
        }
    }
}