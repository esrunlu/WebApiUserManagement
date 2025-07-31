using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Implements
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _context.Users.Where(u => u.IsActive).ToListAsync();
        }

        public async Task<Users> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        }

        public async Task<Users> AddNewUser(Users user)
        {
            try
            {
                
                user.CreatedDate = DateTime.UtcNow;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception("DB Save Error: " + errorMessage);
            }
        }



        public async Task<bool> UpdateUser(Users user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null || !existing.IsActive)
                return false;

            existing.Name = user.Name;
            existing.Username = user.Username;
            existing.Email = user.Email;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || !user.IsActive)
                return false;

            user.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Users> Login(LoginDto loginDto)
        {
            var member = await _context.Users
                .FirstOrDefaultAsync(m => m.Email.ToLower() == loginDto.Email.ToLower());

            if (member == null)
            {
                Console.WriteLine("Kullanıcı bulunamadı.");
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.EnhancedVerify(loginDto.Password, member.PasswordHash);

            if (!isPasswordValid)
            {
                Console.WriteLine("Şifre uyuşmuyor.");
                return null;
            }

            Console.WriteLine("Giriş başarılı.");
            return member;
        }

        public async Task<bool> SoftDeleteUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || !user.IsActive)
                return false;

            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Users>> GetAllUsersOrderByDate()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .OrderByDescending(u => u.CreatedDate)
                .ToListAsync();
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public async Task<List<UserWithRoleDto>> GetUsersWithRolesFromSP()
        {
            return await _context.UsersWithRolesDto.FromSqlRaw("EXEC GetUsersWithRoles").ToListAsync();
        }

        public async Task AddUserRole(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
        }
       




    }
}



