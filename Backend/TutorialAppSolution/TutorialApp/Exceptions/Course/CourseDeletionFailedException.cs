namespace TutorialApp.Exceptions.Course
{
    public class CourseDeletionFailedException : Exception
    {
        string message;

        
        public CourseDeletionFailedException()
        {
            message = "Course deletion failed";
        }
        public CourseDeletionFailedException(string message)
        {
            this.message = message;
        }

        public override string Message => message;

    }
}
