namespace Tutorial_App.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string Status { get; set; } // Registered, Started, Completed
        public DateTime? CompletionDate { get; set; }

        public User User { get; set; }
        public Course Course { get; set; }
    }
}
