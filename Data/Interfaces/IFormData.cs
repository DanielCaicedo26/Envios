using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IFormData : IBaseModelData<Form>
    {
        Task<bool> ActiveAsync(int id, bool status);
        Task<bool> UpdatePartial(Form form);
        Task<Form> GetByNameAsync(string name);
        Task<List<Form>> GetFormsByModuleIdAsync(int moduleId);
        Task<List<Form>> SearchFormsAsync(string searchTerm);
    }
}
