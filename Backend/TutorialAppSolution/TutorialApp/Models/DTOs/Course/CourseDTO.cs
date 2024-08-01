namespace TutorialApp.Models.DTOs.Course
{
    public class CourseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }

        public string CourseURL { get; set; }
        public decimal Price { get; set; }
        public string? CourseImageUrl { get; set; }

        public IFormFile Image { get; set; }
        public string InstructorName { get; set; }
    }
}
