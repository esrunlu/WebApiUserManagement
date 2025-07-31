using Microsoft.EntityFrameworkCore;
using WebApplication1.Dtos;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        // DTO için DbSet tanımı (Key'siz, sadece SP sonucu için)
        public DbSet<UserWithRoleDto> UsersWithRolesDto { get; set; }

        public async Task<List<UserWithRoleDto>> GetUsersWithRolesFromSP()
        {
            return await UsersWithRolesDto
                .FromSqlRaw("EXEC GetUsersWithRoles")
                .ToListAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key ve ilişkiler UserRole için
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // DTO için Key tanımı yok, view gibi kullan
            modelBuilder.Entity<UserWithRoleDto>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null); // Bu, Db'de gerçek bir tablo olmadığını belirtir
            });
        }
    }
}
