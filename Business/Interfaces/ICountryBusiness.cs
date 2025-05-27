using Entity.Dtos.CountryDTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    namespace Business.Interfaces
    {
        public interface ICountryBusiness : IBaseBusiness<Country, CountryDto>
        {
            Task<bool> UpdatePartialCountryAsync(UpdateCountryDto dto);
            Task<bool> DeleteLogicCountryAsync(DeleteLogicalCountryDto dto);
            Task<CountryDto> GetByNameAsync(string name);
        }
    }
}
