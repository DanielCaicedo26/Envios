using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.ModuleFormDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface IModuleFormController : IGenericController<ModuleFormDto, ModuleForm>
    {
        Task<IActionResult> UpdatePartialModuleForm(UpdateModuleFormDto dto);
        Task<IActionResult> DeleteLogicModuleForm(int id);
        Task<IActionResult> GetByModuleAndForm(int moduleId, int formId);
        Task<IActionResult> GetByModuleId(int moduleId);
        Task<IActionResult> GetByFormId(int formId);
        Task<IActionResult> AssignFormToModule(int moduleId, int formId);
        Task<IActionResult> RemoveFormFromModule(int moduleId, int formId);
    }
}