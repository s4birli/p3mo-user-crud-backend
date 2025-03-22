using Microsoft.EntityFrameworkCore;
using p3mo_user_crud_backend.Data;

namespace p3mo_user_crud_backend.Services
{
    public class StatsService : IStatsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StatsService> _logger;

        public StatsService(
            ApplicationDbContext context, 
            ILogger<StatsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<object> GetActiveUserStatsAsync()
        {
            _logger.LogInformation("Getting active user statistics");
            
            var activeCount = await _context.Users.CountAsync(u => u.IsActive);
            var inactiveCount = await _context.Users.CountAsync(u => !u.IsActive);

            return new
            {
                Active = activeCount,
                Inactive = inactiveCount,
                Total = activeCount + inactiveCount
            };
        }

        public async Task<object> GetRoleDistributionStatsAsync()
        {
            _logger.LogInformation("Getting role distribution statistics");
            
            var roleCounts = await _context.UserDetails
                .Include(ud => ud.Role)
                .GroupBy(ud => ud.Role!.Name)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .ToListAsync();

            return roleCounts;
        }

        public async Task<object> GetRegistrationStatsAsync()
        {
            _logger.LogInformation("Getting user registration statistics");
            
            var registrationStats = await _context.Users
                .GroupBy(u => new { Month = u.CreatedAt.Month, Year = u.CreatedAt.Year })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            return registrationStats;
        }
    }
} 