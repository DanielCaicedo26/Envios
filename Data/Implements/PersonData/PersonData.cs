using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements.PersonData
{
    public class PersonData : BaseModelData<Person>, IPersonData
    {
        public PersonData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var person = await _context.Set<Person>().FindAsync(id);
            if (person == null)
                return false;

            person.Status = status;
            _context.Entry(person).Property(p => p.Status).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(Person person)
        {
            var existingPerson = await _context.Set<Person>().FindAsync(person.Id);
            if (existingPerson == null) return false;

            // Update only the fields that are not null or empty
            if (!string.IsNullOrEmpty(person.FirstName))
                existingPerson.FirstName = person.FirstName;
            if (!string.IsNullOrEmpty(person.LastName))
                existingPerson.LastName = person.LastName;
            if (!string.IsNullOrEmpty(person.DocumentType))
                existingPerson.DocumentType = person.DocumentType;
            if (!string.IsNullOrEmpty(person.DocumentNumber))
                existingPerson.DocumentNumber = person.DocumentNumber;
            if (!string.IsNullOrEmpty(person.PhoneNumber))
                existingPerson.PhoneNumber = person.PhoneNumber;

            if (person.CountryId > 0) existingPerson.CountryId = person.CountryId;
            if (person.DepartmentId > 0) existingPerson.DepartmentId = person.DepartmentId;
            if (person.CityId > 0) existingPerson.CityId = person.CityId;
            if (person.NeighborhoodId > 0) existingPerson.NeighborhoodId = person.NeighborhoodId;

            _context.Set<Person>().Update(existingPerson);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Person> GetByDocumentAsync(string documentType, string documentNumber)
        {
            return await _context.Set<Person>()
                .FirstOrDefaultAsync(p => p.DocumentType == documentType
                                    && p.DocumentNumber == documentNumber
                                    && p.Status);
        }

        public async Task<Person> GetByPhoneAsync(string phoneNumber)
        {
            return await _context.Set<Person>()
                .FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber && p.Status);
        }

        public async Task<List<Person>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null)
        {
            var query = _context.Set<Person>().Where(p => p.CountryId == countryId && p.Status);

            if (departmentId.HasValue)
                query = query.Where(p => p.DepartmentId == departmentId.Value);

            if (cityId.HasValue)
                query = query.Where(p => p.CityId == cityId.Value);

            return await query.ToListAsync();
        }
    }
}