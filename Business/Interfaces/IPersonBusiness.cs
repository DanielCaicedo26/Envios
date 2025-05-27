using Entity.Dtos.PersonDTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IPersonBusiness : IBaseBusiness<Person, PersonDto>
    {
        Task<bool> UpdatePartialPersonAsync(UpdatePersonDto dto);
        Task<bool> DeleteLogicPersonAsync(DeleteLogicalPersonDto dto);
        Task<PersonDto> GetByDocumentAsync(string documentType, string documentNumber);
        Task<PersonDto> GetByPhoneAsync(string phoneNumber);
        Task<List<PersonDto>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null);
    }
}
