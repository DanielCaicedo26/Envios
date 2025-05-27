using Entity.Dtos.RolFormPermissionDTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRolFormPermissionBusiness : IBaseBusiness<RolFormPermission, RolFormPermissionDto>
    {
        Task<bool> UpdatePartialRolFormPermissionAsync(UpdateRolFormPermissionDto dto);
        Task<bool> DeleteLogicRolFormPermissionAsync(DeleteLogicalRolFormPermissionDto dto);
        Task<RolFormPermissionDto> GetByRoleFormPermissionAsync(int roleId, int formId, int permissionId);
        Task<List<RolFormPermissionDto>> GetByRoleIdAsync(int roleId);
        Task<List<RolFormPermissionDto>> GetByFormIdAsync(int formId);
        Task<List<RolFormPermissionDto>> GetByPermissionIdAsync(int permissionId);
        Task<bool> AssignPermissionToRoleFormAsync(int roleId, int formId, int permissionId, bool canCreate = false, bool canRead = false, bool canUpdate = false, bool canDelete = false);
        Task<bool> UpdateCRUDPermissionsAsync(int roleId, int formId, int permissionId, bool canCreate, bool canRead, bool canUpdate, bool canDelete);
        Task<bool> RemovePermissionFromRoleFormAsync(int roleId, int formId, int permissionId);
        Task<bool> HasPermissionAsync(int roleId, int formId, string permissionType);
    }
}
