using TutorialApp.Models;

namespace TutorialApp.Models.DTOs.Enrollment
{
    public class EnrollmentDTO
    {
        public string UserEmail { get; set; }
        public int CourseId { get; set; }

        public string Status { get; set; }
        
    }
}