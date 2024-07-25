using System.ComponentModel.DataAnnotations;

namespace TutorialApp.Models.DTOs.User
{
    public class UserProfileDTO
    {
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        public IFormFile Image { get; set; }

        public string? ImageUri { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}
