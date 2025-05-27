using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.FormDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class FormBusiness : BaseBusiness<Form, FormDto>, IFormBusiness
    {
        private readonly IFormData _formData;

        public FormBusiness(IFormData formData, IMapper mapper, ILogger<FormBusiness> logger, IGenericIHelpers helpers)
            : base(formData, mapper, logger, helpers)
        {
            _formData = formData;
        }

        public async Task<bool> UpdatePartialFormAsync(UpdateFormDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var form = _mapper.Map<Form>(dto);
            return await _formData.UpdatePartial(form);
        }

        public async Task<bool> DeleteLogicFormAsync(DeleteLogicalFormDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del formulario es inválido");

            var exists = await _formData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("form", dto.Id);

            return await _formData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<FormDto> GetByNameAsync(string name)
        {
            var form = await _formData.GetByNameAsync(name);
            return _mapper.Map<FormDto>(form);
        }

        public async Task<List<FormDto>> GetFormsByModuleIdAsync(int moduleId)
        {
            var forms = await _formData.GetFormsByModuleIdAsync(moduleId);
            return _mapper.Map<List<FormDto>>(forms);
        }

        public async Task<List<FormDto>> SearchFormsAsync(string searchTerm)
        {
            var forms = await _formData.SearchFormsAsync(searchTerm);
            return _mapper.Map<List<FormDto>>(forms);
        }
    }
}
