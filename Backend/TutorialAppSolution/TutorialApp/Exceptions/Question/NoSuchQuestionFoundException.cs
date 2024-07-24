namespace TutorialApp.Exceptions.Question
{
    public class NoSuchQuestionFoundException : Exception
    {
        string message;

        public NoSuchQuestionFoundException()
        {
            this.message = "No such question found";
        }
        public NoSuchQuestionFoundException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
