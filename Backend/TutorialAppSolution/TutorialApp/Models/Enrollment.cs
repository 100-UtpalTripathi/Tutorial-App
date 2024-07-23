using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorialApp.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public string Status { get; set; } // Registered, Started, Completed

        public DateTime? EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }

        [ForeignKey("UserEmail")]
        public User User { get; set; }
        public Course Course { get; set; }
    }
}
