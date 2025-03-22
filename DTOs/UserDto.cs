using System.ComponentModel.DataAnnotations;

namespace p3mo_user_crud_backend.DTOs
{
    // DTO representing the combined User and UserDetails data for API responses
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Role { get; set; } = string.Empty; // Role name for display
        public int RoleId { get; set; } // Role ID for reference
        public string Country { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }

    // DTO for role information
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    // DTO for creating a new user with details
    public class CreateUserDto
    {
        // User properties
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        // UserDetails properties
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? MiddleName { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid role")]
        public int RoleId { get; set; } = 2; // Default to User role (ID: 2)
        
        [Required]
        [StringLength(50)]
        public string Country { get; set; } = string.Empty;
        
        [Url]
        public string? AvatarUrl { get; set; }
    }

    // DTO for updating an existing user with details
    public class UpdateUserDto
    {
        // User properties
        [EmailAddress]
        public string? Email { get; set; }
        
        public bool? IsActive { get; set; }
        
        // UserDetails properties
        [StringLength(50, MinimumLength = 2)]
        public string? FirstName { get; set; }
        
        [StringLength(50)]
        public string? MiddleName { get; set; }
        
        [StringLength(50, MinimumLength = 2)]
        public string? LastName { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid role")]
        public int? RoleId { get; set; }
        
        [StringLength(50)]
        public string? Country { get; set; }
        
        [Url]
        public string? AvatarUrl { get; set; }
    }
} 