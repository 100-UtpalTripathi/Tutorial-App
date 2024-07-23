using System.ComponentModel.DataAnnotations;

namespace TutorialApp.Models
{
    public class Module
    {
        [Key]
        [Required]
        public int ModuleId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }


        public Course Course { get; set; }
    }

}
