using Microsoft.EntityFrameworkCore;
using p3mo_user_crud_backend.Data;
using p3mo_user_crud_backend.DTOs;
using p3mo_user_crud_backend.Models;

namespace p3mo_user_crud_backend.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            ApplicationDbContext context,
            ILogger<RoleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            _logger.LogInformation("Getting all roles");
            
            var roles = await _context.Roles.ToListAsync();
            return roles.Select(MapRoleToDto);
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            _logger.LogInformation("Getting role with ID {RoleId}", id);
            
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return null;
                
            return MapRoleToDto(role);
        }

        public async Task<RoleDto> CreateRoleAsync(RoleDto roleDto)
        {
            _logger.LogInformation("Creating new role: {RoleName}", roleDto.Name);
            
            var role = new Role
            {
                Name = roleDto.Name,
                Description = roleDto.Description
            };
            
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            
            return MapRoleToDto(role);
        }

        public async Task<RoleDto?> UpdateRoleAsync(int id, RoleDto roleDto)
        {
            _logger.LogInformation("Updating role with ID {RoleId}", id);
            
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return null;
                
            // Update properties
            role.Name = roleDto.Name;
            role.Description = roleDto.Description;
            
            await _context.SaveChangesAsync();
            
            return MapRoleToDto(role);
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            _logger.LogInformation("Deleting role with ID {RoleId}", id);
            
            // Check if role is assigned to any user
            var isRoleInUse = await _context.UserDetails.AnyAsync(ud => ud.RoleId == id);
            if (isRoleInUse)
            {
                _logger.LogWarning("Cannot delete role with ID {RoleId} as it is assigned to users", id);
                return false;
            }
            
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return false;
                
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        private RoleDto MapRoleToDto(Role role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }
    }
} 