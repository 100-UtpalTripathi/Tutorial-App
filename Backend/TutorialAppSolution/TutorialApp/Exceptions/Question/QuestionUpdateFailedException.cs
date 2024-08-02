namespace TutorialApp.Exceptions.Question
{
    public class QuestionUpdateFailedException : Exception
    {
        string message;

        public QuestionUpdateFailedException()
        {
            message = "An error occurred while updating the question.";
        }
        public QuestionUpdateFailedException(string message, Exception ex)
        {
            this.message = message;
        }
        public  override string Message => message;
        
    }
}
