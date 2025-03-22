using p3mo_user_crud_backend.DTOs;

namespace p3mo_user_crud_backend.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<RoleDto> CreateRoleAsync(RoleDto roleDto);
        Task<RoleDto?> UpdateRoleAsync(int id, RoleDto roleDto);
        Task<bool> DeleteRoleAsync(int id);
    }
} 