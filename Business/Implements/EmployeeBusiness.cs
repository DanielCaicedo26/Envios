using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.EmployeeDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class EmployeeBusiness : BaseBusiness<Employee, EmployeeDto>, IEmployeeBusiness
    {
        private readonly IEmployeeData _employeeData;

        public EmployeeBusiness(IEmployeeData employeeData, IMapper mapper, ILogger<EmployeeBusiness> logger, IGenericIHelpers helpers)
            : base(employeeData, mapper, logger, helpers)
        {
            _employeeData = employeeData;
        }

        public async Task<bool> UpdatePartialEmployeeAsync(UpdateEmployeeDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var employee = _mapper.Map<Employee>(dto);
            return await _employeeData.UpdatePartial(employee);
        }

        public async Task<bool> DeleteLogicEmployeeAsync(DeleteLogicalEmployeeDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del empleado es inválido");

            var exists = await _employeeData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("employee", dto.Id);

            return await _employeeData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<EmployeeDto> GetByEmployeeCodeAsync(string employeeCode)
        {
            var employee = await _employeeData.GetByEmployeeCodeAsync(employeeCode);
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<List<EmployeeDto>> GetBySupervisorIdAsync(int supervisorId)
        {
            var employees = await _employeeData.GetBySupervisorIdAsync(supervisorId);
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<List<EmployeeDto>> GetActiveEmployeesAsync()
        {
            var employees = await _employeeData.GetActiveEmployeesAsync();
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<List<EmployeeDto>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null)
        {
            var employees = await _employeeData.GetByLocationAsync(countryId, departmentId, cityId);
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<bool> AssignSupervisorAsync(int employeeId, int supervisorId)
        {
            return await _employeeData.AssignSupervisorAsync(employeeId, supervisorId);
        }
    }
}