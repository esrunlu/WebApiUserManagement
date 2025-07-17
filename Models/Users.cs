namespace WebApplication1.Models
{
    public class Users
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }

        public bool IsActive { get; set; }
        public DateTime InsertDate { get; set; }
        public string? Password { get; internal set; }
    }

}

