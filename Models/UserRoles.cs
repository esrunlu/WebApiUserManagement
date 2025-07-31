using System.Data;

namespace WebApplication1.Models
{
    public class UserRole
    {
        public int UserId { get; set; }
        public Users User { get; set; }

        public int RoleId { get; set; }
        public Roles Role { get; set; }
    }
}

