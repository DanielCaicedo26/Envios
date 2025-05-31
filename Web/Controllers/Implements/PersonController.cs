using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.PersonDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class PersonController : GenericController<PersonDto, Person>, IPersonController
    {
        private readonly IPersonBusiness _personBusiness;

        public PersonController(IPersonBusiness personBusiness, ILogger<PersonController> logger)
            : base(personBusiness, logger)
        {
            _personBusiness = personBusiness;
        }

        protected override int GetEntityId(PersonDto dto)
        {
            return dto.Id;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialPerson([FromBody] UpdatePersonDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _personBusiness.UpdatePartialPersonAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente persona: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente persona: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("logic/{id}")]
        public async Task<IActionResult> DeleteLogicPerson(int id)
        {
            try
            {
                var dto = new DeleteLogicalPersonDto { Id = id, Status = false };
                var result = await _personBusiness.DeleteLogicPersonAsync(dto);
                if (!result)
                    return NotFound($"Persona con ID {id} no encontrada");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al eliminar lógicamente persona: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar lógicamente persona con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("document")]
        public async Task<IActionResult> GetByDocument([FromQuery] string documentType, [FromQuery] string documentNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(documentType) || string.IsNullOrWhiteSpace(documentNumber))
                    return BadRequest("Tipo y número de documento son requeridos");

                var person = await _personBusiness.GetByDocumentAsync(documentType, documentNumber);
                if (person == null)
                    return NotFound($"Persona con documento {documentType} {documentNumber} no encontrada");

                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener persona por documento {documentType} {documentNumber}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("phone/{phoneNumber}")]
        public async Task<IActionResult> GetByPhone(string phoneNumber)
        {
            try
            {
                var person = await _personBusiness.GetByPhoneAsync(phoneNumber);
                if (person == null)
                    return NotFound($"Persona con teléfono {phoneNumber} no encontrada");

                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener persona por teléfono {phoneNumber}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("location")]
        public async Task<IActionResult> GetByLocation([FromQuery] int countryId, [FromQuery] int? departmentId = null, [FromQuery] int? cityId = null)
        {
            try
            {
                var people = await _personBusiness.GetByLocationAsync(countryId, departmentId, cityId);
                return Ok(people);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener personas por ubicación: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}