using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Users user);
    }
}

