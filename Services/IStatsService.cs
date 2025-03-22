using p3mo_user_crud_backend.DTOs;

namespace p3mo_user_crud_backend.Services
{
    public interface IStatsService
    {
        Task<object> GetActiveUserStatsAsync();
        Task<object> GetRoleDistributionStatsAsync();
        Task<object> GetRegistrationStatsAsync();
    }
} 