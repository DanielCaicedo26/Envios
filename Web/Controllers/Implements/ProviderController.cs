using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.ProviderDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class ProviderController : GenericController<ProviderDto, Provider>, IProviderController
    {
        private readonly IProviderBusiness _providerBusiness;

        public ProviderController(IProviderBusiness providerBusiness, ILogger<ProviderController> logger)
            : base(providerBusiness, logger)
        {
            _providerBusiness = providerBusiness;
        }

        protected override int GetEntityId(ProviderDto dto)
        {
            return dto.Id;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialProvider([FromBody] UpdateProviderDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _providerBusiness.UpdatePartialProviderAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente proveedor: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente proveedor: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("logic/{id}")]
        public async Task<IActionResult> DeleteLogicProvider(int id)
        {
            try
            {
                var dto = new DeleteLogicalProviderDto { Id = id, Status = false };
                var result = await _providerBusiness.DeleteLogicProviderAsync(dto);
                if (!result)
                    return NotFound($"Proveedor con ID {id} no encontrado");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al eliminar lógicamente proveedor: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar lógicamente proveedor con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("provider-code/{providerCode}")]
        public async Task<IActionResult> GetByProviderCode(string providerCode)
        {
            try
            {
                var provider = await _providerBusiness.GetByProviderCodeAsync(providerCode);
                if (provider == null)
                    return NotFound($"Proveedor con código {providerCode} no encontrado");

                return Ok(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener proveedor por código {providerCode}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("tax-id/{taxId}")]
        public async Task<IActionResult> GetByTaxId(string taxId)
        {
            try
            {
                var provider = await _providerBusiness.GetByTaxIdAsync(taxId);
                if (provider == null)
                    return NotFound($"Proveedor con NIT {taxId} no encontrado");

                return Ok(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener proveedor por NIT {taxId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var provider = await _providerBusiness.GetByEmailAsync(email);
                if (provider == null)
                    return NotFound($"Proveedor con email {email} no encontrado");

                return Ok(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener proveedor por email {email}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("location")]
        public async Task<IActionResult> GetByLocation([FromQuery] int countryId, [FromQuery] int? departmentId = null, [FromQuery] int? cityId = null)
        {
            try
            {
                var providers = await _providerBusiness.GetByLocationAsync(countryId, departmentId, cityId);
                return Ok(providers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener proveedores por ubicación: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("business-type/{businessType}")]
        public async Task<IActionResult> GetByBusinessType(string businessType)
        {
            try
            {
                var providers = await _providerBusiness.GetByBusinessTypeAsync(businessType);
                return Ok(providers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener proveedores por tipo de negocio {businessType}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}