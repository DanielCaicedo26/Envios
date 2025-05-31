using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.RolFormPermissionDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface IRolFormPermissionController : IGenericController<RolFormPermissionDto, RolFormPermission>
    {
        Task<IActionResult> UpdatePartialRolFormPermission(UpdateRolFormPermissionDto dto);
        Task<IActionResult> DeleteLogicRolFormPermission(int id);
        Task<IActionResult> GetByRoleFormPermission(int roleId, int formId, int permissionId);
        Task<IActionResult> GetByRoleId(int roleId);
        Task<IActionResult> GetByFormId(int formId);
        Task<IActionResult> GetByPermissionId(int permissionId);
        Task<IActionResult> AssignPermissionToRoleForm(int roleId, int formId, int permissionId, bool canCreate = false, bool canRead = false, bool canUpdate = false, bool canDelete = false);
        Task<IActionResult> UpdateCRUDPermissions(int roleId, int formId, int permissionId, bool canCreate, bool canRead, bool canUpdate, bool canDelete);
        Task<IActionResult> RemovePermissionFromRoleForm(int roleId, int formId, int permissionId);
        Task<IActionResult> HasPermission(int roleId, int formId, string permissionType);
    }
}