using Entity.Dtos.ModuleFormDTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IModuleFormBusiness : IBaseBusiness<ModuleForm, ModuleFormDto>
    {
        Task<bool> UpdatePartialModuleFormAsync(UpdateModuleFormDto dto);
        Task<bool> DeleteLogicModuleFormAsync(DeleteLogicalModuleFormDto dto);
        Task<ModuleFormDto> GetByModuleAndFormAsync(int moduleId, int formId);
        Task<List<ModuleFormDto>> GetByModuleIdAsync(int moduleId);
        Task<List<ModuleFormDto>> GetByFormIdAsync(int formId);
        Task<bool> AssignFormToModuleAsync(int moduleId, int formId);
        Task<bool> RemoveFormFromModuleAsync(int moduleId, int formId);
    }
}
