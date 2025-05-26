using Data.Interfaces;
using Entity.Model.Base;

public interface IRolFormPermissionData : IBaseModelData<RolFormPermission>
{
    Task<bool> ActiveAsync(int id, bool status);
    Task<bool> UpdatePartial(RolFormPermission rolFormPermission);
    Task<RolFormPermission> GetByRoleFormPermissionAsync(int roleId, int formId, int permissionId);
    Task<List<RolFormPermission>> GetByRoleIdAsync(int roleId);
    Task<List<RolFormPermission>> GetByFormIdAsync(int formId);
    Task<List<RolFormPermission>> GetByPermissionIdAsync(int permissionId);
    Task<bool> AssignPermissionToRoleFormAsync(int roleId, int formId, int permissionId, bool canCreate = false, bool canRead = false, bool canUpdate = false, bool canDelete = false);
    Task<bool> UpdateCRUDPermissionsAsync(int roleId, int formId, int permissionId, bool canCreate, bool canRead, bool canUpdate, bool canDelete);
    Task<bool> RemovePermissionFromRoleFormAsync(int roleId, int formId, int permissionId);
    Task<bool> HasPermissionAsync(int roleId, int formId, string permissionType);
}