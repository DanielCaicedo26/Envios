using Entity.Dtos.ModuleDTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IModuleBusiness : IBaseBusiness<Module, ModuleDto>
    {
        Task<bool> UpdatePartialModuleAsync(UpdateModuleDto dto);
        Task<bool> DeleteLogicModuleAsync(DeleteLogicalModuleDto dto);
        Task<ModuleDto> GetByNameAsync(string name);
        Task<List<ModuleDto>> GetModulesByFormIdAsync(int formId);
        Task<List<ModuleDto>> SearchModulesAsync(string searchTerm);
    }
}
