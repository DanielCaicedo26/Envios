using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements.ProviderData
{
    public class ProviderData : BaseModelData<Provider>, IProviderData
    {
        public ProviderData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var provider = await _context.Set<Provider>().FindAsync(id);
            if (provider == null)
                return false;

            provider.Status = status;
            _context.Entry(provider).Property(p => p.Status).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(Provider provider)
        {
            var existingProvider = await _context.Set<Provider>().FindAsync(provider.Id);
            if (existingProvider == null) return false;

            // Update only the fields that are not null or empty
            if (!string.IsNullOrEmpty(provider.ProviderCode))
                existingProvider.ProviderCode = provider.ProviderCode;
            if (!string.IsNullOrEmpty(provider.CompanyName))
                existingProvider.CompanyName = provider.CompanyName;
            if (!string.IsNullOrEmpty(provider.TaxId))
                existingProvider.TaxId = provider.TaxId;
            if (!string.IsNullOrEmpty(provider.BusinessType))
                existingProvider.BusinessType = provider.BusinessType;
            if (!string.IsNullOrEmpty(provider.Website))
                existingProvider.Website = provider.Website;
            if (!string.IsNullOrEmpty(provider.ContactEmail))
                existingProvider.ContactEmail = provider.ContactEmail;
            if (!string.IsNullOrEmpty(provider.ContactPhone))
                existingProvider.ContactPhone = provider.ContactPhone;
            if (!string.IsNullOrEmpty(provider.ProductsServices))
                existingProvider.ProductsServices = provider.ProductsServices;
            if (!string.IsNullOrEmpty(provider.PaymentTerms))
                existingProvider.PaymentTerms = provider.PaymentTerms;

            // Update foreign keys if provided
            if (provider.PersonId > 0) existingProvider.PersonId = provider.PersonId;
            if (provider.CountryId > 0) existingProvider.CountryId = provider.CountryId;
            if (provider.DepartmentId > 0) existingProvider.DepartmentId = provider.DepartmentId;
            if (provider.CityId > 0) existingProvider.CityId = provider.CityId;
            if (provider.NeighborhoodId > 0) existingProvider.NeighborhoodId = provider.NeighborhoodId;

            _context.Set<Provider>().Update(existingProvider);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Provider> GetByProviderCodeAsync(string providerCode)
        {
            return await _context.Set<Provider>()
                .FirstOrDefaultAsync(p => p.ProviderCode == providerCode && p.Status);
        }

        public async Task<Provider> GetByTaxIdAsync(string taxId)
        {
            return await _context.Set<Provider>()
                .FirstOrDefaultAsync(p => p.TaxId == taxId && p.Status);
        }

        public async Task<Provider> GetByEmailAsync(string email)
        {
            return await _context.Set<Provider>()
                .FirstOrDefaultAsync(p => p.ContactEmail.ToLower() == email.ToLower() && p.Status);
        }

        public async Task<List<Provider>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null)
        {
            var query = _context.Set<Provider>().Where(p => p.CountryId == countryId && p.Status);

            if (departmentId.HasValue)
                query = query.Where(p => p.DepartmentId == departmentId.Value);

            if (cityId.HasValue)
                query = query.Where(p => p.CityId == cityId.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Provider>> GetByBusinessTypeAsync(string businessType)
        {
            return await _context.Set<Provider>()
                .Where(p => p.BusinessType.ToLower().Contains(businessType.ToLower()) && p.Status)
                .ToListAsync();
        }
    }
}