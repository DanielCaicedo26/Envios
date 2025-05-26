using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements.ModuleData
{
    public class ModuleData : BaseModelData<Module>, IModuleData
    {
        public ModuleData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var module = await _context.Set<Module>().FindAsync(id);
            if (module == null)
                return false;

            module.Status = status;
            _context.Entry(module).Property(m => m.Status).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(Module module)
        {
            var existingModule = await _context.Set<Module>().FindAsync(module.Id);
            if (existingModule == null) return false;

            // Update only the fields that are not null or empty
            if (!string.IsNullOrEmpty(module.Name))
                existingModule.Name = module.Name;
            if (!string.IsNullOrEmpty(module.Description))
                existingModule.Description = module.Description;

            _context.Set<Module>().Update(existingModule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Module> GetByNameAsync(string name)
        {
            return await _context.Set<Module>()
                .FirstOrDefaultAsync(m => m.Name.ToLower() == name.ToLower() && m.Status);
        }

        public async Task<List<Module>> GetModulesByFormIdAsync(int formId)
        {
            return await _context.Set<ModuleForm>()
                .Where(mf => mf.FormId == formId && mf.Status)
                .Include(mf => mf.Module)
                .Select(mf => mf.Module)
                .Where(m => m.Status)
                .ToListAsync();
        }

        public async Task<List<Module>> SearchModulesAsync(string searchTerm)
        {
            return await _context.Set<Module>()
                .Where(m => (m.Name.ToLower().Contains(searchTerm.ToLower()) ||
                            m.Description.ToLower().Contains(searchTerm.ToLower()))
                            && m.Status)
                .ToListAsync();
        }
    }
}