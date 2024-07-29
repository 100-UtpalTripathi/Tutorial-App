namespace TutorialApp.Models.DTOs.Enrollment
{
    public class EnrollmentFailedException : Exception
    {
        string message;

        public EnrollmentFailedException()
        {
            message = "Enrollment failed.";
        }

        public EnrollmentFailedException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
