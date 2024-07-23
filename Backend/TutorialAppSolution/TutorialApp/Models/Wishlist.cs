using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorialApp.Models
{
    public class Wishlist
    {
        [Key]
        public int WishlistId { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public int CourseId { get; set; }

        [ForeignKey("UserEmail")]
        public User User { get; set; }
        public Course Course { get; set; }
    }

}
