namespace TutorialApp.Exceptions.Quiz
{
    public class QuizCreationFailedException : Exception
    {
        string message;

        public QuizCreationFailedException()
        {
            message = "Quiz creation failed";
        }

        public QuizCreationFailedException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
