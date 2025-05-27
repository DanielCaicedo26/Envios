using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IPersonData : IBaseModelData<Person>
    {
        Task<bool> ActiveAsync(int id, bool status);
        Task<bool> UpdatePartial(Person person);
        Task<Person> GetByDocumentAsync(string documentType, string documentNumber);
        Task<Person> GetByPhoneAsync(string phoneNumber);
        Task<List<Person>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null);
    }
}
