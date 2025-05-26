using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface INeighborhoodData : IBaseModelData<Neighborhood>
    {
        Task<bool> ActiveAsync(int id, bool status);
        Task<bool> UpdatePartial(Neighborhood neighborhood);
        Task<List<Neighborhood>> GetByCityIdAsync(int cityId);
        Task<Neighborhood> GetByNameAsync(string name);
    }
}
