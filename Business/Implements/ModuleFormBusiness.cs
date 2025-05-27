using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.ModuleFormDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class ModuleFormBusiness : BaseBusiness<ModuleForm, ModuleFormDto>, IModuleFormBusiness
    {
        private readonly IModuleFormData _moduleFormData;

        public ModuleFormBusiness(IModuleFormData moduleFormData, IMapper mapper, ILogger<ModuleFormBusiness> logger, IGenericIHelpers helpers)
            : base(moduleFormData, mapper, logger, helpers)
        {
            _moduleFormData = moduleFormData;
        }

        public async Task<bool> UpdatePartialModuleFormAsync(UpdateModuleFormDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            if (dto.FormId <= 0 && dto.ModuleId <= 0)
                throw new ArgumentException("Debe enviar al menos un campo para actualizar.");

            var moduleForm = _mapper.Map<ModuleForm>(dto);
            return await _moduleFormData.UpdatePartial(moduleForm);
        }

        public async Task<bool> DeleteLogicModuleFormAsync(DeleteLogicalModuleFormDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID de la relación módulo-formulario es inválido");

            var exists = await _moduleFormData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("moduleForm", dto.Id);

            return await _moduleFormData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<ModuleFormDto> GetByModuleAndFormAsync(int moduleId, int formId)
        {
            var moduleForm = await _moduleFormData.GetByModuleAndFormAsync(moduleId, formId);
            return _mapper.Map<ModuleFormDto>(moduleForm);
        }

        public async Task<List<ModuleFormDto>> GetByModuleIdAsync(int moduleId)
        {
            var moduleForms = await _moduleFormData.GetByModuleIdAsync(moduleId);
            return _mapper.Map<List<ModuleFormDto>>(moduleForms);
        }

        public async Task<List<ModuleFormDto>> GetByFormIdAsync(int formId)
        {
            var moduleForms = await _moduleFormData.GetByFormIdAsync(formId);
            return _mapper.Map<List<ModuleFormDto>>(moduleForms);
        }

        public async Task<bool> AssignFormToModuleAsync(int moduleId, int formId)
        {
            return await _moduleFormData.AssignFormToModuleAsync(moduleId, formId);
        }

        public async Task<bool> RemoveFormFromModuleAsync(int moduleId, int formId)
        {
            return await _moduleFormData.RemoveFormFromModuleAsync(moduleId, formId);
        }
    }
}