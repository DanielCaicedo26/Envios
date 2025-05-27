using Entity.Dtos.DepartmentDTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IDepartmentBusiness : IBaseBusiness<Department, DepartmentDto>
    {
        Task<bool> UpdatePartialDepartmentAsync(UpdateDepartmentDto dto);
        Task<bool> DeleteLogicDepartmentAsync(DeleteLogicalDepartmentDto dto);
        Task<List<DepartmentDto>> GetByCountryIdAsync(int countryId);
        Task<DepartmentDto> GetByNameAsync(string name);
    }
}
