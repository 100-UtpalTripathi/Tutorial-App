namespace TutorialApp.Exceptions.User
{
    public class NoSuchUserFoundException : Exception
    {
        string message;

        public NoSuchUserFoundException()
        {
            this.message = "No such user found";
        }
        public NoSuchUserFoundException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
