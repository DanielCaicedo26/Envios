using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.ClientDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class ClientController : GenericController<ClientDto, Client>, IClientController
    {
        private readonly IClientBusiness _clientBusiness;

        public ClientController(IClientBusiness clientBusiness, ILogger<ClientController> logger)
            : base(clientBusiness, logger)
        {
            _clientBusiness = clientBusiness;
        }

        protected override int GetEntityId(ClientDto dto)
        {
            return dto.Id;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialClient([FromBody] UpdateClientDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _clientBusiness.UpdatePartialClientAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente cliente: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente cliente: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("logic/{id}")]
        public async Task<IActionResult> DeleteLogicClient(int id)
        {
            try
            {
                var dto = new DeleteLogicalClientDto { Id = id, Status = false };
                var result = await _clientBusiness.DeleteLogicClientAsync(dto);
                if (!result)
                    return NotFound($"Cliente con ID {id} no encontrado");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al eliminar lógicamente cliente: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar lógicamente cliente con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("client-code/{clientCode}")]
        public async Task<IActionResult> GetByClientCode(string clientCode)
        {
            try
            {
                var client = await _clientBusiness.GetByClientCodeAsync(clientCode);
                if (client == null)
                    return NotFound($"Cliente con código {clientCode} no encontrado");

                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener cliente por código {clientCode}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("tax-id/{taxId}")]
        public async Task<IActionResult> GetByTaxId(string taxId)
        {
            try
            {
                var client = await _clientBusiness.GetByTaxIdAsync(taxId);
                if (client == null)
                    return NotFound($"Cliente con NIT {taxId} no encontrado");

                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener cliente por NIT {taxId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var client = await _clientBusiness.GetByEmailAsync(email);
                if (client == null)
                    return NotFound($"Cliente con email {email} no encontrado");

                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener cliente por email {email}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("location")]
        public async Task<IActionResult> GetByLocation([FromQuery] int countryId, [FromQuery] int? departmentId = null, [FromQuery] int? cityId = null)
        {
            try
            {
                var clients = await _clientBusiness.GetByLocationAsync(countryId, departmentId, cityId);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener clientes por ubicación: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("credit-limit")]
        public async Task<IActionResult> GetByCreditLimitRange([FromQuery] decimal minLimit, [FromQuery] decimal maxLimit)
        {
            try
            {
                var clients = await _clientBusiness.GetByCreditLimitRangeAsync(minLimit, maxLimit);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener clientes por rango de crédito: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}