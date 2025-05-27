using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements.PermissionData
{
    public class PermissionData : BaseModelData<Permission>, IPermissionData
    {
        public PermissionData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var permission = await _context.Set<Permission>().FindAsync(id);
            if (permission == null)
                return false;

            permission.Status = status;
            _context.Entry(permission).Property(p => p.Status).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(Permission permission)
        {
            var existingPermission = await _context.Set<Permission>().FindAsync(permission.Id);
            if (existingPermission == null) return false;

            // Update only the fields that are not null or empty
            if (!string.IsNullOrEmpty(permission.Name))
                existingPermission.Name = permission.Name;
            if (!string.IsNullOrEmpty(permission.Description))
                existingPermission.Description = permission.Description;

            _context.Set<Permission>().Update(existingPermission);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Permission> GetByNameAsync(string name)
        {
            return await _context.Set<Permission>()
                .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower() && p.Status);
        }

        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            return await _context.Set<RolFormPermission>()
                .Where(rfp => rfp.RolId == roleId && rfp.Status)
                .Include(rfp => rfp.Permission)
                .Select(rfp => rfp.Permission)
                .Where(p => p.Status)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Permission>> SearchPermissionsAsync(string searchTerm)
        {
            return await _context.Set<Permission>()
                .Where(p => (p.Name.ToLower().Contains(searchTerm.ToLower()) ||
                            p.Description.ToLower().Contains(searchTerm.ToLower()))
                            && p.Status)
                .ToListAsync();
        }

        public async Task<List<Permission>> GetBasicPermissionsAsync()
        {
            var basicPermissions = new[] { "Create", "Read", "Update", "Delete" };
            return await _context.Set<Permission>()
                .Where(p => basicPermissions.Contains(p.Name) && p.Status)
                .ToListAsync();
        }
    }
}