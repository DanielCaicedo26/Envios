using Data.Implements.BaseData;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements.FormData
{
    public class FormData : BaseModelData<Form>, IFormData
    {
        public FormData(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ActiveAsync(int id, bool status)
        {
            var form = await _context.Set<Form>().FindAsync(id);
            if (form == null)
                return false;

            form.Status = status;
            _context.Entry(form).Property(f => f.Status).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartial(Form form)
        {
            var existingForm = await _context.Set<Form>().FindAsync(form.Id);
            if (existingForm == null) return false;

            // Update only the fields that are not null or empty
            if (!string.IsNullOrEmpty(form.Name))
                existingForm.Name = form.Name;
            if (!string.IsNullOrEmpty(form.Description))
                existingForm.Description = form.Description;

            _context.Set<Form>().Update(existingForm);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Form> GetByNameAsync(string name)
        {
            return await _context.Set<Form>()
                .FirstOrDefaultAsync(f => f.Name.ToLower() == name.ToLower() && f.Status);
        }

        public async Task<List<Form>> GetFormsByModuleIdAsync(int moduleId)
        {
            return await _context.Set<ModuleForm>()
                .Where(mf => mf.ModuleId == moduleId && mf.Status)
                .Include(mf => mf.Form)
                .Select(mf => mf.Form)
                .Where(f => f.Status)
                .ToListAsync();
        }

        public async Task<List<Form>> SearchFormsAsync(string searchTerm)
        {
            return await _context.Set<Form>()
                .Where(f => (f.Name.ToLower().Contains(searchTerm.ToLower()) ||
                            f.Description.ToLower().Contains(searchTerm.ToLower()))
                            && f.Status)
                .ToListAsync();
        }
    }
}