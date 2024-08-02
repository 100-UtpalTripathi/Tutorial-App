namespace TutorialApp.Exceptions.Question
{
    public class QuestionDeleteFailedException : Exception
    {
        string message;

        public QuestionDeleteFailedException()
        {
            message = "An error occurred while deleting the question.";
        }
        public QuestionDeleteFailedException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
