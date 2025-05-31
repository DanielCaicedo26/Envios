using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.NeighborhoodDTO;
using Entity.Model.Base;

namespace Web.Controllers.Interface
{
    public interface INeighborhoodController : IGenericController<NeighborhoodDto, Neighborhood>
    {
        Task<IActionResult> UpdatePartialNeighborhood(UpdateNeighborhoodDto dto);
        Task<IActionResult> DeleteLogicNeighborhood(int id);
        Task<IActionResult> GetByCityId(int cityId);
        Task<IActionResult> GetByName(string name);
    }
}