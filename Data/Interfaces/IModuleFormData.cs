using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IModuleFormData : IBaseModelData<ModuleForm>
    {
        Task<bool> ActiveAsync(int id, bool status);
        Task<bool> UpdatePartial(ModuleForm moduleForm);
        Task<ModuleForm> GetByModuleAndFormAsync(int moduleId, int formId);
        Task<List<ModuleForm>> GetByModuleIdAsync(int moduleId);
        Task<List<ModuleForm>> GetByFormIdAsync(int formId);
        Task<bool> AssignFormToModuleAsync(int moduleId, int formId);
        Task<bool> RemoveFormFromModuleAsync(int moduleId, int formId);
    }
}
