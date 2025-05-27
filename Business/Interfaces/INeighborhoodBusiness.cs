using Entity.Dtos.NeighborhoodDTO;
using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface INeighborhoodBusiness : IBaseBusiness<Neighborhood, NeighborhoodDto>
    {
        Task<bool> UpdatePartialNeighborhoodAsync(UpdateNeighborhoodDto dto);
        Task<bool> DeleteLogicNeighborhoodAsync(DeleteLogicalNeighborhoodDto dto);
        Task<List<NeighborhoodDto>> GetByCityIdAsync(int cityId);
        Task<NeighborhoodDto> GetByNameAsync(string name);
    }
}
