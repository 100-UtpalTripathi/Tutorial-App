namespace TutorialApp.Models.DTOs.Enrollment
{
    public class EnrolledCoursesDTO
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string CategoryName { get; set; }
        public decimal Price { get; set; }

        public string CourseImageUrl { get; set; }

        public String InstructorName { get; set; }

        public string CourseURL { get; set; }

        public string? Status { get; set; }  // Registered or Completed
    }
}
