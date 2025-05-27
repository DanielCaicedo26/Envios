using Entity.Dtos.PermissionDTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IPermissionBusiness : IBaseBusiness<Permission, PermissionDto>
    {
        Task<bool> UpdatePartialPermissionAsync(UpdatePermissionDto dto);
        Task<bool> DeleteLogicPermissionAsync(DeleteLogicalPermissionDto dto);
        Task<PermissionDto> GetByNameAsync(string name);
        Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(int roleId);
        Task<List<PermissionDto>> SearchPermissionsAsync(string searchTerm);
        Task<List<PermissionDto>> GetBasicPermissionsAsync();
    }
}
