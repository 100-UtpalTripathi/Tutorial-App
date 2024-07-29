namespace TutorialApp.Exceptions.User
{
    public class UserProfileUpdateFailedException : Exception
    {
        string message;

        public UserProfileUpdateFailedException()
        {
            message = "User profile update failed";
        }

        public UserProfileUpdateFailedException(string message) { this.message = message; }

        public override string Message => message;
    }
}
