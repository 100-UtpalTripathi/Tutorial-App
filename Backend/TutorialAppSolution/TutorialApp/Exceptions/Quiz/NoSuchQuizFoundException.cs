namespace TutorialApp.Exceptions.Quiz
{
    public class NoSuchQuizFoundException : Exception
    {
        string message;

        public NoSuchQuizFoundException()
        {
            this.message = "No such quiz found";
        }
        public NoSuchQuizFoundException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
