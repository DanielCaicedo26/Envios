using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.FormDTO;
using Entity.Model;

namespace Web.Controllers.Interface
{
    public interface IFormController : IGenericController<FormDto, Form>
    {
        Task<IActionResult> UpdatePartialForm(UpdateFormDto dto);
        Task<IActionResult> DeleteLogicForm(int id);
        Task<IActionResult> GetByName(string name);
        Task<IActionResult> GetFormsByModuleId(int moduleId);
        Task<IActionResult> SearchForms(string searchTerm);
    }
}