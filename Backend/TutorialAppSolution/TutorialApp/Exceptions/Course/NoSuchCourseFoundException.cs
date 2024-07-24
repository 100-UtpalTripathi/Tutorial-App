namespace TutorialApp.Exceptions.Course
{
    public class NoSuchCourseFoundException : Exception
    {
        string message;

        public NoSuchCourseFoundException()
        {
            this.message = "No such course found";
        }
        public NoSuchCourseFoundException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
