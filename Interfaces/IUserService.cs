using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Dtos;
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
        Task<Users> Login(LoginDto loginDto);
        Task<bool> SoftDeleteUserById(int id);
        Task<List<Users>> GetAllUsersOrderByDate();
        string HashPassword(string password);
        //Task<string> HashPassword(string password);
        Task<List<UserWithRoleDto>> GetUsersWithRolesFromSP();
        Task AddUserRole(UserRole userRole);


    }
}

