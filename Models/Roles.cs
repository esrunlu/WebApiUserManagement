namespace WebApplication1.Models
{
    public class Roles
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Users> Users { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
