using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements.NeighborhoodData
{
    public class NeighborhoodData : BaseModelData<Neighborhood>, INeighborhoodData
    {
        public NeighborhoodData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var neighborhood = await _context.Set<Neighborhood>().FindAsync(id);
            if (neighborhood == null)
                return false;

            neighborhood.Status = status;
            _context.Entry(neighborhood).Property(n => n.Status).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(Neighborhood neighborhood)
        {
            var existingNeighborhood = await _context.Set<Neighborhood>().FindAsync(neighborhood.Id);
            if (existingNeighborhood == null) return false;

            // Update only the fields that are not null or empty
            if (!string.IsNullOrEmpty(neighborhood.Name))
                existingNeighborhood.Name = neighborhood.Name;
            if (!string.IsNullOrEmpty(neighborhood.Code))
                existingNeighborhood.Code = neighborhood.Code;
            if (neighborhood.CityId > 0)
                existingNeighborhood.CityId = neighborhood.CityId;

            _context.Set<Neighborhood>().Update(existingNeighborhood);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Neighborhood>> GetByCityIdAsync(int cityId)
        {
            return await _context.Set<Neighborhood>()
                .Where(n => n.CityId == cityId && n.Status)
                .ToListAsync();
        }

        public async Task<Neighborhood> GetByNameAsync(string name)
        {
            return await _context.Set<Neighborhood>()
                .FirstOrDefaultAsync(n => n.Name.ToLower() == name.ToLower() && n.Status);
        }
    }
}