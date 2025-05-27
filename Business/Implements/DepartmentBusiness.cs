using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.DepartmentDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class DepartmentBusiness : BaseBusiness<Department, DepartmentDto>, IDepartmentBusiness
    {
        private readonly IDepartmentData _departmentData;

        public DepartmentBusiness(IDepartmentData departmentData, IMapper mapper, ILogger<DepartmentBusiness> logger, IGenericIHelpers helpers)
            : base(departmentData, mapper, logger, helpers)
        {
            _departmentData = departmentData;
        }

        public async Task<bool> UpdatePartialDepartmentAsync(UpdateDepartmentDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var department = _mapper.Map<Department>(dto);
            return await _departmentData.UpdatePartial(department);
        }

        public async Task<bool> DeleteLogicDepartmentAsync(DeleteLogicalDepartmentDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del departamento es inválido");

            var exists = await _departmentData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("department", dto.Id);

            return await _departmentData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<List<DepartmentDto>> GetByCountryIdAsync(int countryId)
        {
            var departments = await _departmentData.GetByCountryIdAsync(countryId);
            return _mapper.Map<List<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto> GetByNameAsync(string name)
        {
            var department = await _departmentData.GetByNameAsync(name);
            return _mapper.Map<DepartmentDto>(department);
        }
    }
}