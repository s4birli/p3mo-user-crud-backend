using Microsoft.EntityFrameworkCore;
using p3mo_user_crud_backend.Data;
using p3mo_user_crud_backend.DTOs;
using p3mo_user_crud_backend.Models;

namespace p3mo_user_crud_backend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(
            ApplicationDbContext context, 
            ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.UserDetails)
                .ThenInclude(ud => ud.Role)
                .ToListAsync();
                
            return users.Select(MapUserToDto);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserDetails)
                .ThenInclude(ud => ud.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
                
            if (user == null)
                return null;
            
            return MapUserToDto(user);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
        {
            // Create the User entity
            var user = new User
            {
                Email = userDto.Email,
                IsActive = userDto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            // Create the UserDetails entity
            var userDetails = new UserDetails
            {
                FirstName = userDto.FirstName,
                MiddleName = userDto.MiddleName,
                LastName = userDto.LastName,
                DateOfBirth = userDto.DateOfBirth,
                RoleId = userDto.RoleId,
                Country = userDto.Country,
            };

            // Set the relationship
            user.UserDetails = userDetails;

            // Add the User (UserDetails will be added automatically through the relationship)
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Fetch the user with role for the DTO
            var createdUser = await _context.Users
                .Include(u => u.UserDetails)
                .ThenInclude(ud => ud.Role)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return MapUserToDto(createdUser!);
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto userDto)
        {
            var user = await _context.Users
                .Include(u => u.UserDetails)
                .FirstOrDefaultAsync(u => u.Id == id);
                
            if (user == null)
                return null;

            // Update User properties
            if (userDto.Email != null)
                user.Email = userDto.Email;
            
            if (userDto.IsActive.HasValue)
                user.IsActive = userDto.IsActive.Value;
            
            // Ensure UserDetails exists
            if (user.UserDetails == null)
            {
                user.UserDetails = new UserDetails
                {
                    UserId = user.Id
                };
            }
            
            // Update UserDetails properties
            if (userDto.FirstName != null)
                user.UserDetails.FirstName = userDto.FirstName;
                
            if (userDto.LastName != null)
                user.UserDetails.LastName = userDto.LastName;
                
            if (userDto.MiddleName != null)
                user.UserDetails.MiddleName = userDto.MiddleName;
            
            if (userDto.DateOfBirth.HasValue)
                user.UserDetails.DateOfBirth = userDto.DateOfBirth.Value;
            
            if (userDto.RoleId.HasValue)
                user.UserDetails.RoleId = userDto.RoleId.Value;
            
            if (userDto.Country != null)
                user.UserDetails.Country = userDto.Country;

            await _context.SaveChangesAsync();
            
            // Fetch updated user with role information
            var updatedUser = await _context.Users
                .Include(u => u.UserDetails)
                .ThenInclude(ud => ud.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
                
            return MapUserToDto(updatedUser!);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private UserDto MapUserToDto(User user)
        {
            if (user.UserDetails == null)
            {
                // If UserDetails doesn't exist, return minimal DTO
                return new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    // Set default values for UserDetails fields
                    FirstName = "Unknown",
                    LastName = "Unknown",
                    MiddleName = null,
                    DateOfBirth = DateTime.MinValue,
                    RoleId = 2, // Default to User role
                    Role = "User", // Default role name
                    Country = "Unknown",
                };
            }
            
            // Otherwise, return complete DTO
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                // UserDetails fields
                FirstName = user.UserDetails.FirstName,
                LastName = user.UserDetails.LastName,
                MiddleName = user.UserDetails.MiddleName,
                DateOfBirth = user.UserDetails.DateOfBirth,
                RoleId = user.UserDetails.RoleId,
                Role = user.UserDetails.Role?.Name ?? "Unknown",
                Country = user.UserDetails.Country
            };
        }
    }
} 