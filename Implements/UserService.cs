using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using System.Security.Cryptography;
using System.Text;

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
            user.Password = HashPassword(user.Password); // şifre hash’leniyor
            user.IsActive = true;
            user.InsertDate = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
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

        public async Task<bool> Login(string email, string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);
                var hashedPassword = Convert.ToBase64String(hashBytes);

                var user = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Email == email && u.Password == hashedPassword && u.IsActive);

                return user != null;
            }
        }


        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        object IUserService.HashPassword(string password)
        {
            return HashPassword(password);
        }
    }
}
   

