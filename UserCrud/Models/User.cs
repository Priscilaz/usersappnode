using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserCrud.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
