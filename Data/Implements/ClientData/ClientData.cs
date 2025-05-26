using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements.ClientData
{
    public class ClientData : BaseModelData<Client>, IClientData
    {
        public ClientData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var client = await _context.Set<Client>().FindAsync(id);
            if (client == null)
                return false;

            client.Status = status;
            _context.Entry(client).Property(c => c.Status).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(Client client)
        {
            var existingClient = await _context.Set<Client>().FindAsync(client.Id);
            if (existingClient == null) return false;

            // Update only the fields that are not null or empty
            if (!string.IsNullOrEmpty(client.ClientCode))
                existingClient.ClientCode = client.ClientCode;
            if (!string.IsNullOrEmpty(client.CompanyName))
                existingClient.CompanyName = client.CompanyName;
            if (!string.IsNullOrEmpty(client.TaxId))
                existingClient.TaxId = client.TaxId;
            if (!string.IsNullOrEmpty(client.BusinessType))
                existingClient.BusinessType = client.BusinessType;
            if (!string.IsNullOrEmpty(client.Website))
                existingClient.Website = client.Website;
            if (!string.IsNullOrEmpty(client.ContactEmail))
                existingClient.ContactEmail = client.ContactEmail;
            if (!string.IsNullOrEmpty(client.ContactPhone))
                existingClient.ContactPhone = client.ContactPhone;
            if (client.CreditLimit > 0)
                existingClient.CreditLimit = client.CreditLimit;
            if (!string.IsNullOrEmpty(client.PaymentTerms))
                existingClient.PaymentTerms = client.PaymentTerms;

            // Update foreign keys if provided
            if (client.PersonId > 0) existingClient.PersonId = client.PersonId;
            if (client.CountryId > 0) existingClient.CountryId = client.CountryId;
            if (client.DepartmentId > 0) existingClient.DepartmentId = client.DepartmentId;
            if (client.CityId > 0) existingClient.CityId = client.CityId;
            if (client.NeighborhoodId > 0) existingClient.NeighborhoodId = client.NeighborhoodId;

            _context.Set<Client>().Update(existingClient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Client> GetByClientCodeAsync(string clientCode)
        {
            return await _context.Set<Client>()
                .FirstOrDefaultAsync(c => c.ClientCode == clientCode && c.Status);
        }

        public async Task<Client> GetByTaxIdAsync(string taxId)
        {
            return await _context.Set<Client>()
                .FirstOrDefaultAsync(c => c.TaxId == taxId && c.Status);
        }

        public async Task<Client> GetByEmailAsync(string email)
        {
            return await _context.Set<Client>()
                .FirstOrDefaultAsync(c => c.ContactEmail.ToLower() == email.ToLower() && c.Status);
        }

        public async Task<List<Client>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null)
        {
            var query = _context.Set<Client>().Where(c => c.CountryId == countryId && c.Status);

            if (departmentId.HasValue)
                query = query.Where(c => c.DepartmentId == departmentId.Value);

            if (cityId.HasValue)
                query = query.Where(c => c.CityId == cityId.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Client>> GetByCreditLimitRangeAsync(decimal minLimit, decimal maxLimit)
        {
            return await _context.Set<Client>()
                .Where(c => c.CreditLimit >= minLimit && c.CreditLimit <= maxLimit && c.Status)
                .ToListAsync();
        }
    }
}