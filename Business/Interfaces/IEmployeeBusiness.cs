using Entity.Dtos.EmployeeDTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IEmployeeBusiness : IBaseBusiness<Employee, EmployeeDto>
    {
        Task<bool> UpdatePartialEmployeeAsync(UpdateEmployeeDto dto);
        Task<bool> DeleteLogicEmployeeAsync(DeleteLogicalEmployeeDto dto);
        Task<EmployeeDto> GetByEmployeeCodeAsync(string employeeCode);
        Task<List<EmployeeDto>> GetBySupervisorIdAsync(int supervisorId);
        Task<List<EmployeeDto>> GetActiveEmployeesAsync();
        Task<List<EmployeeDto>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null);
        Task<bool> AssignSupervisorAsync(int employeeId, int supervisorId);
    }
}
