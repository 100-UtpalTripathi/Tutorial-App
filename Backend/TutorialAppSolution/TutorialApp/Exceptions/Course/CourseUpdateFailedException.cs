namespace TutorialApp.Exceptions.Course
{
    public class CourseUpdateFailedException : Exception
    {
        string message;

        public CourseUpdateFailedException()
        {
            message = "Course update failed";
        }
        public CourseUpdateFailedException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
