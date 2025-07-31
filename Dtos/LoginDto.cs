using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Dtos
{
    [Table ("Users")]
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }



    }
}
