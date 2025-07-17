using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IUserService
    {
        Task<List<Users>> GetAllUsers();
        Task<Users> GetUserById(int id);
        Task<Users> AddNewUser(Users user);
        Task<bool> UpdateUser(Users user);
        Task<bool> DeleteUserById(int id);
        Task<bool> Login(string email, string password);
        object HashPassword(string password);
    }
}
