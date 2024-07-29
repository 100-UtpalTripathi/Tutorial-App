namespace TutorialApp.Exceptions.Quiz
{
    public class QuizDeletionFailedException : Exception
    {
        string message;

        public QuizDeletionFailedException()
        {
            message = "Quiz deletion failed";
        }

        public QuizDeletionFailedException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
        
    }
}
