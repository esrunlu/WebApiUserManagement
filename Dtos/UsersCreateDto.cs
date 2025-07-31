using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Dtos
{
    [Table ("Users")]
    public class UsersCreateDto
    {
        
        public string Name { get; set; }

        
        public string Username { get; set; }

        
        public string Email { get; set; }

        
        public string PasswordHash { get; set; }
    }
}

