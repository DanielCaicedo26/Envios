using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IPermissionData : IBaseModelData<Permission>
    {
        Task<bool> ActiveAsync(int id, bool status);
        Task<bool> UpdatePartial(Permission permission);
        Task<Permission> GetByNameAsync(string name);
        Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId);
        Task<List<Permission>> SearchPermissionsAsync(string searchTerm);
        Task<List<Permission>> GetBasicPermissionsAsync();
    }
}
