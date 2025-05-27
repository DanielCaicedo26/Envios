using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements.CityData
{
    public class CityData : BaseModelData<City>, ICityData
    {
        public CityData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var city = await _context.Set<City>().FindAsync(id);
            if (city == null)
                return false;

            city.Status = status;
            _context.Entry(city).Property(c => c.Status).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(City city)
        {
            var existingCity = await _context.Set<City>().FindAsync(city.Id);
            if (existingCity == null) return false;

            // Update only the fields that are not null or empty
            if (!string.IsNullOrEmpty(city.Name))
                existingCity.Name = city.Name;
            if (!string.IsNullOrEmpty(city.Code))
                existingCity.Code = city.Code;
            if (!string.IsNullOrEmpty(city.PostalCode))
                existingCity.PostalCode = city.PostalCode;
            if (city.DepartmentId > 0)
                existingCity.DepartmentId = city.DepartmentId;

            _context.Set<City>().Update(existingCity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<City>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.Set<City>()
                .Where(c => c.DepartmentId == departmentId && c.Status)
                .ToListAsync();
        }

        public async Task<City> GetByNameAsync(string name)
        {
            return await _context.Set<City>()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower() && c.Status);
        }

        public async Task<City> GetByPostalCodeAsync(string postalCode)
        {
            return await _context.Set<City>()
                .FirstOrDefaultAsync(c => c.PostalCode == postalCode && c.Status);
        }
    }
}