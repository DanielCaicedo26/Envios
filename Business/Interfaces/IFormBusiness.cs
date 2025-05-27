using Entity.Dtos.FormDTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IFormBusiness : IBaseBusiness<Form, FormDto>
    {
        Task<bool> UpdatePartialFormAsync(UpdateFormDto dto);
        Task<bool> DeleteLogicFormAsync(DeleteLogicalFormDto dto);
        Task<FormDto> GetByNameAsync(string name);
        Task<List<FormDto>> GetFormsByModuleIdAsync(int moduleId);
        Task<List<FormDto>> SearchFormsAsync(string searchTerm);
    }
}
