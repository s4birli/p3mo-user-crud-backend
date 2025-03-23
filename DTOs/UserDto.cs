using System.ComponentModel.DataAnnotations;

namespace p3mo_user_crud_backend.DTOs
{
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
        public string Role { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Country { get; set; } = string.Empty;
    }

    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

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
        public int RoleId { get; set; } = 2;

        [Required]
        [StringLength(50)]
        public string Country { get; set; } = string.Empty;
    }

    public class UpdateUserDto
    {
        [EmailAddress]
        public string? Email { get; set; }

        public bool? IsActive { get; set; }

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
    }
} 