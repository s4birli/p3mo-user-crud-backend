using Microsoft.EntityFrameworkCore;
using p3mo_user_crud_backend.Models;

namespace p3mo_user_crud_backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserDetails> UserDetails { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-one relationship between User and UserDetails
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserDetails)
                .WithOne(ud => ud.User)
                .HasForeignKey<UserDetails>(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Configure one-to-many relationship between Role and UserDetails
            modelBuilder.Entity<Role>()
                .HasMany(r => r.UserDetails)
                .WithOne(ud => ud.Role)
                .HasForeignKey(ud => ud.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", Description = "Administrator with full access" },
                new Role { Id = 2, Name = "User", Description = "Standard user with limited access" },
                new Role { Id = 3, Name = "Guest", Description = "Guest user with read-only access" }
            );

            // Seed initial data for Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "mehmet@example.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed initial data for UserDetails
            modelBuilder.Entity<UserDetails>().HasData(
                new UserDetails
                {
                    Id = 1,
                    UserId = 1,
                    FirstName = "Mehmet",
                    MiddleName = "",
                    LastName = "Sabirli",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    RoleId = 1, // Admin
                    Country = "United Kingdom"
                }
            );
        }
    }
} 