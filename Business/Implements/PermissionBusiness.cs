using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.PermissionDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class PermissionBusiness : BaseBusiness<Permission, PermissionDto>, IPermissionBusiness
    {
        private readonly IPermissionData _permissionData;

        public PermissionBusiness(IPermissionData permissionData, IMapper mapper, ILogger<PermissionBusiness> logger, IGenericIHelpers helpers)
            : base(permissionData, mapper, logger, helpers)
        {
            _permissionData = permissionData;
        }

        public async Task<bool> UpdatePartialPermissionAsync(UpdatePermissionDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var permission = _mapper.Map<Permission>(dto);
            return await _permissionData.UpdatePartial(permission);
        }

        public async Task<bool> DeleteLogicPermissionAsync(DeleteLogicalPermissionDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del permiso es inválido");

            var exists = await _permissionData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("permission", dto.Id);

            return await _permissionData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<PermissionDto> GetByNameAsync(string name)
        {
            var permission = await _permissionData.GetByNameAsync(name);
            return _mapper.Map<PermissionDto>(permission);
        }

        public async Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(int roleId)
        {
            var permissions = await _permissionData.GetPermissionsByRoleIdAsync(roleId);
            return _mapper.Map<List<PermissionDto>>(permissions);
        }

        public async Task<List<PermissionDto>> SearchPermissionsAsync(string searchTerm)
        {
            var permissions = await _permissionData.SearchPermissionsAsync(searchTerm);
            return _mapper.Map<List<PermissionDto>>(permissions);
        }

        public async Task<List<PermissionDto>> GetBasicPermissionsAsync()
        {
            var permissions = await _permissionData.GetBasicPermissionsAsync();
            return _mapper.Map<List<PermissionDto>>(permissions);
        }
    }
}