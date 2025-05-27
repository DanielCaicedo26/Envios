using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements.RolFormPermissionData
{
    public class RolFormPermissionData : BaseModelData<RolFormPermission>, IRolFormPermissionData
    {
        public RolFormPermissionData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var rolFormPermission = await _context.Set<RolFormPermission>().FindAsync(id);
            if (rolFormPermission == null)
                return false;

            rolFormPermission.Status = status;
            _context.Entry(rolFormPermission).Property(rfp => rfp.Status).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(RolFormPermission rolFormPermission)
        {
            var existing = await _context.Set<RolFormPermission>().FindAsync(rolFormPermission.Id);
            if (existing == null) return false;

            // Update only the fields that are provided
            if (rolFormPermission.RolId > 0)
                existing.RolId = rolFormPermission.RolId;
            if (rolFormPermission.FormId > 0)
                existing.FormId = rolFormPermission.FormId;
            if (rolFormPermission.PermissionId > 0)
                existing.PermissionId = rolFormPermission.PermissionId;

            // Update CRUD permissions
            existing.CanCreate = rolFormPermission.CanCreate;
            existing.CanRead = rolFormPermission.CanRead;
            existing.CanUpdate = rolFormPermission.CanUpdate;
            existing.CanDelete = rolFormPermission.CanDelete;

            _context.Set<RolFormPermission>().Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RolFormPermission> GetByRoleFormPermissionAsync(int roleId, int formId, int permissionId)
        {
            return await _context.Set<RolFormPermission>()
                .FirstOrDefaultAsync(rfp => rfp.RolId == roleId
                                         && rfp.FormId == formId
                                         && rfp.PermissionId == permissionId
                                         && rfp.Status);
        }

        public async Task<List<RolFormPermission>> GetByRoleIdAsync(int roleId)
        {
            return await _context.Set<RolFormPermission>()
                .Where(rfp => rfp.RolId == roleId && rfp.Status)
                .Include(rfp => rfp.Rol)
                .Include(rfp => rfp.Permission)
                .ToListAsync();
        }

        public async Task<List<RolFormPermission>> GetByFormIdAsync(int formId)
        {
            return await _context.Set<RolFormPermission>()
                .Where(rfp => rfp.FormId == formId && rfp.Status)
                .Include(rfp => rfp.Rol)
                .Include(rfp => rfp.Permission)
                .ToListAsync();
        }

        public async Task<List<RolFormPermission>> GetByPermissionIdAsync(int permissionId)
        {
            return await _context.Set<RolFormPermission>()
                .Where(rfp => rfp.PermissionId == permissionId && rfp.Status)
                .Include(rfp => rfp.Rol)
                .ToListAsync();
        }

        public async Task<bool> AssignPermissionToRoleFormAsync(int roleId, int formId, int permissionId, bool canCreate = false, bool canRead = false, bool canUpdate = false, bool canDelete = false)
        {
            // Verificar si ya existe la relación
            var existing = await GetByRoleFormPermissionAsync(roleId, formId, permissionId);
            if (existing != null) return false; // Ya existe

            // Verificar que existan el rol, formulario y permiso
            var role = await _context.Roles.FindAsync(roleId);
            var form = await _context.Set<Form>().FindAsync(formId);
            var permission = await _context.Set<Permission>().FindAsync(permissionId);

            if (role == null || form == null || permission == null) return false;

            var rolFormPermission = new RolFormPermission
            {
                RolId = roleId,
                FormId = formId,
                PermissionId = permissionId,
                CanCreate = canCreate,
                CanRead = canRead,
                CanUpdate = canUpdate,
                CanDelete = canDelete,
                Status = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Set<RolFormPermission>().AddAsync(rolFormPermission);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCRUDPermissionsAsync(int roleId, int formId, int permissionId, bool canCreate, bool canRead, bool canUpdate, bool canDelete)
        {
            var rolFormPermission = await GetByRoleFormPermissionAsync(roleId, formId, permissionId);
            if (rolFormPermission == null) return false;

            rolFormPermission.CanCreate = canCreate;
            rolFormPermission.CanRead = canRead;
            rolFormPermission.CanUpdate = canUpdate;
            rolFormPermission.CanDelete = canDelete;
            rolFormPermission.UpdatedAt = DateTime.UtcNow;

            _context.Set<RolFormPermission>().Update(rolFormPermission);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePermissionFromRoleFormAsync(int roleId, int formId, int permissionId)
        {
            var rolFormPermission = await GetByRoleFormPermissionAsync(roleId, formId, permissionId);
            if (rolFormPermission == null) return false;

            rolFormPermission.Status = false;
            rolFormPermission.DeleteAt = DateTime.UtcNow;

            _context.Set<RolFormPermission>().Update(rolFormPermission);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasPermissionAsync(int roleId, int formId, string permissionType)
        {
            var rolFormPermissions = await GetByRoleIdAsync(roleId);
            var formPermissions = rolFormPermissions.Where(rfp => rfp.FormId == formId);

            return permissionType.ToLower() switch
            {
                "create" => formPermissions.Any(rfp => rfp.CanCreate),
                "read" => formPermissions.Any(rfp => rfp.CanRead),
                "update" => formPermissions.Any(rfp => rfp.CanUpdate),
                "delete" => formPermissions.Any(rfp => rfp.CanDelete),
                _ => false
            };
        }
    }
}