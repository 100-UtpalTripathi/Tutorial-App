using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TutorialApp.Models
{
    public class UserCredential
    {
        [Key]
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [ForeignKey("Email")]
        public User User { get; set; }

        [Required]
        public byte[] PasswordHashKey { get; set; }

        [Required]
        public byte[]  Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }
    }
}
