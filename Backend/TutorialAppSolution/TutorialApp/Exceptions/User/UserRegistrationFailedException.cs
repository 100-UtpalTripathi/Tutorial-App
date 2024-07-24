namespace TutorialApp.Exceptions.User
{
    public class UserRegistrationFailedException : Exception
    {
        string message;

        public UserRegistrationFailedException()
        {
            this.message = "User Registration Failed";
        }
        public UserRegistrationFailedException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
