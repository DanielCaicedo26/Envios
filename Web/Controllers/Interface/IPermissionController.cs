using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.PermissionDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface IPermissionController : IGenericController<PermissionDto, Permission>
    {
        Task<IActionResult> UpdatePartialPermission(UpdatePermissionDto dto);
        Task<IActionResult> DeleteLogicPermission(int id);
        Task<IActionResult> GetByName(string name);
        Task<IActionResult> GetPermissionsByRoleId(int roleId);
        Task<IActionResult> SearchPermissions(string searchTerm);
        Task<IActionResult> GetBasicPermissions();
    }
}