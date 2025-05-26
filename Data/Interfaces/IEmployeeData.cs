using Entity.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IEmployeeData : IBaseModelData<Employee>
    {
        Task<bool> ActiveAsync(int id, bool status);
        Task<bool> UpdatePartial(Employee employee);
        Task<Employee> GetByEmployeeCodeAsync(string employeeCode);
        Task<List<Employee>> GetBySupervisorIdAsync(int supervisorId);
        Task<List<Employee>> GetActiveEmployeesAsync();
        Task<List<Employee>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null);
        Task<bool> AssignSupervisorAsync(int employeeId, int supervisorId);
    }
}
