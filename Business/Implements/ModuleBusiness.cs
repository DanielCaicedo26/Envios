using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.ModuleDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class ModuleBusiness : BaseBusiness<Module, ModuleDto>, IModuleBusiness
    {
        private readonly IModuleData _moduleData;

        public ModuleBusiness(IModuleData moduleData, IMapper mapper, ILogger<ModuleBusiness> logger, IGenericIHelpers helpers)
            : base(moduleData, mapper, logger, helpers)
        {
            _moduleData = moduleData;
        }

        public async Task<bool> UpdatePartialModuleAsync(UpdateModuleDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var module = _mapper.Map<Module>(dto);
            return await _moduleData.UpdatePartial(module);
        }

        public async Task<bool> DeleteLogicModuleAsync(DeleteLogicalModuleDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del módulo es inválido");

            var exists = await _moduleData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("module", dto.Id);

            return await _moduleData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<ModuleDto> GetByNameAsync(string name)
        {
            var module = await _moduleData.GetByNameAsync(name);
            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<List<ModuleDto>> GetModulesByFormIdAsync(int formId)
        {
            var modules = await _moduleData.GetModulesByFormIdAsync(formId);
            return _mapper.Map<List<ModuleDto>>(modules);
        }

        public async Task<List<ModuleDto>> SearchModulesAsync(string searchTerm)
        {
            var modules = await _moduleData.SearchModulesAsync(searchTerm);
            return _mapper.Map<List<ModuleDto>>(modules);
        }
    }
}