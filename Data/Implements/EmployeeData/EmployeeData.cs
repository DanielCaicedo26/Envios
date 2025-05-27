using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements.EmployeeData
{
    public class EmployeeData : BaseModelData<Employee>, IEmployeeData
    {
        public EmployeeData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var employee = await _context.Set<Employee>().FindAsync(id);
            if (employee == null)
                return false;

            employee.Status = status;
            employee.IsActive = status; // También actualizar IsActive
            _context.Entry(employee).Property(e => e.Status).IsModified = true;
            _context.Entry(employee).Property(e => e.IsActive).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(Employee employee)
        {
            var existingEmployee = await _context.Set<Employee>().FindAsync(employee.Id);
            if (existingEmployee == null) return false;

            // Update only the fields that are not null or empty
            if (!string.IsNullOrEmpty(employee.Name))
                existingEmployee.Name = employee.Name;
            if (!string.IsNullOrEmpty(employee.LastName))
                existingEmployee.LastName = employee.LastName;
            if (!string.IsNullOrEmpty(employee.EmployeeCode))
                existingEmployee.EmployeeCode = employee.EmployeeCode;
            if (!string.IsNullOrEmpty(employee.WorkSchedule))
                existingEmployee.WorkSchedule = employee.WorkSchedule;

            // Update IsActive if specified
            existingEmployee.IsActive = employee.IsActive;

            // Update foreign keys if provided
            if (employee.PersonId > 0) existingEmployee.PersonId = employee.PersonId;
            if (employee.CountryId > 0) existingEmployee.CountryId = employee.CountryId;
            if (employee.DepartmentId > 0) existingEmployee.DepartmentId = employee.DepartmentId;
            if (employee.CityId > 0) existingEmployee.CityId = employee.CityId;
            if (employee.NeighborhoodId > 0) existingEmployee.NeighborhoodId = employee.NeighborhoodId;
            if (employee.SupervisorId.HasValue) existingEmployee.SupervisorId = employee.SupervisorId;

            _context.Set<Employee>().Update(existingEmployee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Employee> GetByEmployeeCodeAsync(string employeeCode)
        {
            return await _context.Set<Employee>()
                .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode && e.Status);
        }

        public async Task<List<Employee>> GetBySupervisorIdAsync(int supervisorId)
        {
            return await _context.Set<Employee>()
                .Where(e => e.SupervisorId == supervisorId && e.Status)
                .ToListAsync();
        }

        public async Task<List<Employee>> GetActiveEmployeesAsync()
        {
            return await _context.Set<Employee>()
                .Where(e => e.IsActive && e.Status)
                .ToListAsync();
        }

        public async Task<List<Employee>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null)
        {
            var query = _context.Set<Employee>().Where(e => e.CountryId == countryId && e.Status);

            if (departmentId.HasValue)
                query = query.Where(e => e.DepartmentId == departmentId.Value);

            if (cityId.HasValue)
                query = query.Where(e => e.CityId == cityId.Value);

            return await query.ToListAsync();
        }

        public async Task<bool> AssignSupervisorAsync(int employeeId, int supervisorId)
        {
            var employee = await _context.Set<Employee>().FindAsync(employeeId);
            if (employee == null) return false;

            var supervisor = await _context.Set<Employee>().FindAsync(supervisorId);
            if (supervisor == null) return false;

            employee.SupervisorId = supervisorId;
            _context.Set<Employee>().Update(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}