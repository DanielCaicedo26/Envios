using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.ModuleDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface IModuleController : IGenericController<ModuleDto, Module>
    {
        Task<IActionResult> UpdatePartialModule(UpdateModuleDto dto);
        Task<IActionResult> DeleteLogicModule(int id);
        Task<IActionResult> GetByName(string name);
        Task<IActionResult> GetModulesByFormId(int formId);
        Task<IActionResult> SearchModules(string searchTerm);
    }
}