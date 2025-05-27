using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.RolFormPermissionDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class RolFormPermissionBusiness : BaseBusiness<RolFormPermission, RolFormPermissionDto>, IRolFormPermissionBusiness
    {
        private readonly IRolFormPermissionData _rolFormPermissionData;

        public RolFormPermissionBusiness(IRolFormPermissionData rolFormPermissionData, IMapper mapper, ILogger<RolFormPermissionBusiness> logger, IGenericIHelpers helpers)
            : base(rolFormPermissionData, mapper, logger, helpers)
        {
            _rolFormPermissionData = rolFormPermissionData;
        }

        public async Task<bool> UpdatePartialRolFormPermissionAsync(UpdateRolFormPermissionDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var rolFormPermission = _mapper.Map<RolFormPermission>(dto);
            return await _rolFormPermissionData.UpdatePartial(rolFormPermission);
        }

        public async Task<bool> DeleteLogicRolFormPermissionAsync(DeleteLogicalRolFormPermissionDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del permiso de rol-formulario es inválido");

            var exists = await _rolFormPermissionData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("rolFormPermission", dto.Id);

            return await _rolFormPermissionData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<RolFormPermissionDto> GetByRoleFormPermissionAsync(int roleId, int formId, int permissionId)
        {
            var rolFormPermission = await _rolFormPermissionData.GetByRoleFormPermissionAsync(roleId, formId, permissionId);
            return _mapper.Map<RolFormPermissionDto>(rolFormPermission);
        }

        public async Task<List<RolFormPermissionDto>> GetByRoleIdAsync(int roleId)
        {
            var rolFormPermissions = await _rolFormPermissionData.GetByRoleIdAsync(roleId);
            return _mapper.Map<List<RolFormPermissionDto>>(rolFormPermissions);
        }

        public async Task<List<RolFormPermissionDto>> GetByFormIdAsync(int formId)
        {
            var rolFormPermissions = await _rolFormPermissionData.GetByFormIdAsync(formId);
            return _mapper.Map<List<RolFormPermissionDto>>(rolFormPermissions);
        }

        public async Task<List<RolFormPermissionDto>> GetByPermissionIdAsync(int permissionId)
        {
            var rolFormPermissions = await _rolFormPermissionData.GetByPermissionIdAsync(permissionId);
            return _mapper.Map<List<RolFormPermissionDto>>(rolFormPermissions);
        }

        public async Task<bool> AssignPermissionToRoleFormAsync(int roleId, int formId, int permissionId, bool canCreate = false, bool canRead = false, bool canUpdate = false, bool canDelete = false)
        {
            return await _rolFormPermissionData.AssignPermissionToRoleFormAsync(roleId, formId, permissionId, canCreate, canRead, canUpdate, canDelete);
        }

        public async Task<bool> UpdateCRUDPermissionsAsync(int roleId, int formId, int permissionId, bool canCreate, bool canRead, bool canUpdate, bool canDelete)
        {
            return await _rolFormPermissionData.UpdateCRUDPermissionsAsync(roleId, formId, permissionId, canCreate, canRead, canUpdate, canDelete);
        }

        public async Task<bool> RemovePermissionFromRoleFormAsync(int roleId, int formId, int permissionId)
        {
            return await _rolFormPermissionData.RemovePermissionFromRoleFormAsync(roleId, formId, permissionId);
        }

        public async Task<bool> HasPermissionAsync(int roleId, int formId, string permissionType)
        {
            return await _rolFormPermissionData.HasPermissionAsync(roleId, formId, permissionType);
        }
    }
}