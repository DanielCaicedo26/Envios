using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.PersonDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace Business.Implements
{
    public class PersonBusiness : BaseBusiness<Person, PersonDto>, IPersonBusiness
    {
        private readonly IPersonData _personData;

        public PersonBusiness(IPersonData personData, IMapper mapper, ILogger<PersonBusiness> logger, IGenericIHelpers helpers)
            : base(personData, mapper, logger, helpers)
        {
            _personData = personData;
        }

        public async Task<bool> UpdatePartialPersonAsync(UpdatePersonDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var person = _mapper.Map<Person>(dto);
            return await _personData.UpdatePartial(person);
        }

        public async Task<bool> DeleteLogicPersonAsync(DeleteLogicalPersonDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID de la persona es inválido");

            var exists = await _personData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("person", dto.Id);

            return await _personData.ActiveAsync(dto.Id, dto.Status);
        }

        public async Task<PersonDto> GetByDocumentAsync(string documentType, string documentNumber)
        {
            var person = await _personData.GetByDocumentAsync(documentType, documentNumber);
            return _mapper.Map<PersonDto>(person);
        }

        public async Task<PersonDto> GetByPhoneAsync(string phoneNumber)
        {
            var person = await _personData.GetByPhoneAsync(phoneNumber);
            return _mapper.Map<PersonDto>(person);
        }

        public async Task<List<PersonDto>> GetByLocationAsync(int countryId, int? departmentId = null, int? cityId = null)
        {
            var people = await _personData.GetByLocationAsync(countryId, departmentId, cityId);
            return _mapper.Map<List<PersonDto>>(people);
        }
    }
}