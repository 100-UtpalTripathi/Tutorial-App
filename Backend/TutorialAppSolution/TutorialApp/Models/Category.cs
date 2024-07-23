using System.ComponentModel.DataAnnotations;

namespace TutorialApp.Models
{
    public class Category
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
