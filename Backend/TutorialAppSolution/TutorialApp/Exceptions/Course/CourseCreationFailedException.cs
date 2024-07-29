namespace TutorialApp.Exceptions.Course
{
    public class CourseCreationFailedException : Exception
    {
        string message;

        public CourseCreationFailedException()
        {
            message = "Course creation failed";
        }

        public CourseCreationFailedException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
