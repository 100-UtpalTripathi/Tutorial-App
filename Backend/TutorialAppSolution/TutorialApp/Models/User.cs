using System.ComponentModel.DataAnnotations;

namespace TutorialApp.Models
{
    public class User
    {
        [Key]
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

        public string ImageURI { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; }



        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}
