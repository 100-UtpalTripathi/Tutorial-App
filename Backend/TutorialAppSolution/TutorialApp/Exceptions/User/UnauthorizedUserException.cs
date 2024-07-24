namespace TutorialApp.Exceptions.User
{
    public class UnauthorizedUserException : Exception
    {
        string message;

        public UnauthorizedUserException()
        {
            this.message = "Unauthorized User";
        }
        public UnauthorizedUserException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
