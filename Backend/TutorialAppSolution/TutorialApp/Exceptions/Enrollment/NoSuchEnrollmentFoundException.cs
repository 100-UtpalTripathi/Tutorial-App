namespace TutorialApp.Exceptions.Enrollment
{
    public class NoSuchEnrollmentFoundException : Exception
    {
        string message;

        public NoSuchEnrollmentFoundException()
        {
            this.message = "No such enrollment found";
        }
        public NoSuchEnrollmentFoundException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
