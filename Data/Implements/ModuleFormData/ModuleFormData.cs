using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements.ModuleFormData
{
    public class ModuleFormData : BaseModelData<ModuleForm>, IModuleFormData
    {
        public ModuleFormData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var moduleForm = await _context.Set<ModuleForm>().FindAsync(id);
            if (moduleForm == null)
                return false;

            moduleForm.Status = status;
            _context.Entry(moduleForm).Property(mf => mf.Status).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(ModuleForm moduleForm)
        {
            var existingModuleForm = await _context.Set<ModuleForm>().FindAsync(moduleForm.Id);
            if (existingModuleForm == null) return false;

            // Update only the fields that are provided
            if (moduleForm.FormId > 0)
                existingModuleForm.FormId = moduleForm.FormId;
            if (moduleForm.ModuleId > 0)
                existingModuleForm.ModuleId = moduleForm.ModuleId;

            _context.Set<ModuleForm>().Update(existingModuleForm);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ModuleForm> GetByModuleAndFormAsync(int moduleId, int formId)
        {
            return await _context.Set<ModuleForm>()
                .FirstOrDefaultAsync(mf => mf.ModuleId == moduleId && mf.FormId == formId && mf.Status);
        }

        public async Task<List<ModuleForm>> GetByModuleIdAsync(int moduleId)
        {
            return await _context.Set<ModuleForm>()
                .Where(mf => mf.ModuleId == moduleId && mf.Status)
                .Include(mf => mf.Form)
                .ToListAsync();
        }

        public async Task<List<ModuleForm>> GetByFormIdAsync(int formId)
        {
            return await _context.Set<ModuleForm>()
                .Where(mf => mf.FormId == formId && mf.Status)
                .Include(mf => mf.Module)
                .ToListAsync();
        }

        public async Task<bool> AssignFormToModuleAsync(int moduleId, int formId)
        {
            // Verificar si ya existe la relación
            var existing = await GetByModuleAndFormAsync(moduleId, formId);
            if (existing != null) return false; // Ya existe

            // Verificar que existan el módulo y el formulario
            var module = await _context.Set<Module>().FindAsync(moduleId);
            var form = await _context.Set<Form>().FindAsync(formId);

            if (module == null || form == null) return false;

            var moduleForm = new ModuleForm
            {
                ModuleId = moduleId,
                FormId = formId,
                Status = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Set<ModuleForm>().AddAsync(moduleForm);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFormFromModuleAsync(int moduleId, int formId)
        {
            var moduleForm = await GetByModuleAndFormAsync(moduleId, formId);
            if (moduleForm == null) return false;

            moduleForm.Status = false;
            moduleForm.DeleteAt = DateTime.UtcNow;

            _context.Set<ModuleForm>().Update(moduleForm);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}