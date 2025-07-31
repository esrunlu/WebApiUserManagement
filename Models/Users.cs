using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }  // 

        [Required, MaxLength(100)]
        public string Username { get; set; }  //

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        //public string Role { get; set; }



        public bool IsActive { get; set; } = true;

        public DateTime? CreatedDate { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        
    }
}



