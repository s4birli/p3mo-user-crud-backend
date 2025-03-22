using Microsoft.AspNetCore.Mvc;
using p3mo_user_crud_backend.Services;

namespace p3mo_user_crud_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;
        private readonly ILogger<StatsController> _logger;

        public StatsController(
            IStatsService statsService, 
            ILogger<StatsController> logger)
        {
            _statsService = statsService;
            _logger = logger;
        }

        [HttpGet("active")]
        public async Task<ActionResult> GetActiveUserStats()
        {
            try
            {
                var stats = await _statsService.GetActiveUserStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting active user stats");
                return StatusCode(500, "Internal server error occurred while retrieving active user stats");
            }
        }

        [HttpGet("roles")]
        public async Task<ActionResult> GetRoleDistributionStats()
        {
            try
            {
                var stats = await _statsService.GetRoleDistributionStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting role distribution stats");
                return StatusCode(500, "Internal server error occurred while retrieving role distribution stats");
            }
        }

        [HttpGet("registration")]
        public async Task<ActionResult> GetRegistrationStats()
        {
            try
            {
                var stats = await _statsService.GetRegistrationStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting registration stats");
                return StatusCode(500, "Internal server error occurred while retrieving registration stats");
            }
        }
    }
} 