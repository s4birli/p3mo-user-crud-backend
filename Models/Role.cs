using System.ComponentModel.DataAnnotations;

namespace p3mo_user_crud_backend.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? Description { get; set; }
        
        // Navigation property
        public ICollection<UserDetails> UserDetails { get; set; } = new List<UserDetails>();
    }
} 