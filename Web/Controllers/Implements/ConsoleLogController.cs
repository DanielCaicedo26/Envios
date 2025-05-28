using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Entity.Context;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers.Implements
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConsoleLogController : ControllerBase
    {
        private readonly AuditDbContext _dbcontext;
        private readonly ILogger<ConsoleLogController> _logger;

        public ConsoleLogController(AuditDbContext dbcontext, ILogger<ConsoleLogController> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? tableName = null,
            [FromQuery] string? operationType = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var query = _dbcontext.ConsoleLogs.AsQueryable();

                if (!string.IsNullOrEmpty(tableName))
                    query = query.Where(l => l.TableName.Contains(tableName));

                if (!string.IsNullOrEmpty(operationType))
                    query = query.Where(l => l.OperationType == operationType.ToUpper());

                if (startDate.HasValue)
                    query = query.Where(l => l.Timestamp >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(l => l.Timestamp <= endDate.Value);

                var totalRecords = await query.CountAsync();

                var logs = await query
                    .OrderByDescending(l => l.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    Data = logs,
                    TotalRecords = totalRecords,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener logs: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}