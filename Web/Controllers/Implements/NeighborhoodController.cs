using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.NeighborhoodDTO;
using Entity.Model.Base;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class NeighborhoodController : GenericController<NeighborhoodDto, Neighborhood>, INeighborhoodController
    {
        private readonly INeighborhoodBusiness _neighborhoodBusiness;

        public NeighborhoodController(INeighborhoodBusiness neighborhoodBusiness, ILogger<NeighborhoodController> logger)
            : base(neighborhoodBusiness, logger)
        {
            _neighborhoodBusiness = neighborhoodBusiness;
        }

        protected override int GetEntityId(NeighborhoodDto dto)
        {
            return dto.Id;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialNeighborhood([FromBody] UpdateNeighborhoodDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _neighborhoodBusiness.UpdatePartialNeighborhoodAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente barrio: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente barrio: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("logic/{id}")]
        public async Task<IActionResult> DeleteLogicNeighborhood(int id)
        {
            try
            {
                var dto = new DeleteLogicalNeighborhoodDto { Id = id, Status = false };
                var result = await _neighborhoodBusiness.DeleteLogicNeighborhoodAsync(dto);
                if (!result)
                    return NotFound($"Barrio con ID {id} no encontrado");

                return Ok(new { Success = true });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al eliminar lógicamente barrio: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar lógicamente barrio con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("city/{cityId}")]
        public async Task<IActionResult> GetByCityId(int cityId)
        {
            try
            {
                var neighborhoods = await _neighborhoodBusiness.GetByCityIdAsync(cityId);
                return Ok(neighborhoods);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener barrios por ciudad {cityId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var neighborhood = await _neighborhoodBusiness.GetByNameAsync(name);
                if (neighborhood == null)
                    return NotFound($"Barrio con nombre {name} no encontrado");

                return Ok(neighborhood);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener barrio por nombre {name}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
