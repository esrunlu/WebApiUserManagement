namespace WebApplication1.Dtos
{
    public class UsersCreateDto
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string Password { get; set; } // Şifre artık burada var!
    }
}
