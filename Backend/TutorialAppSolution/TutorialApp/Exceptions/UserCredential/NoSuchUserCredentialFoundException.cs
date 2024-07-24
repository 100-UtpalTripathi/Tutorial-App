namespace TutorialApp.Exceptions.UserCredential
{
    public class NoSuchUserCredentialFoundException : Exception
    {
        string message;

        public NoSuchUserCredentialFoundException()
        {
            message = "No such user credential found";
        }

        public NoSuchUserCredentialFoundException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
