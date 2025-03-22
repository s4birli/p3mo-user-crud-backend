using System.ComponentModel.DataAnnotations;

namespace p3mo_user_crud_backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for one-to-one relationship with UserDetails
        public UserDetails? UserDetails { get; set; }
    }
} 