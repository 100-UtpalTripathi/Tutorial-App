using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TutorialApp.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public string UserEmail{ get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("UserEmail")]
        public User User { get; set; }
        public Course Course { get; set; }
    }

}
