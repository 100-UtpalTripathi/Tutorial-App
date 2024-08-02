namespace TutorialApp.Exceptions.Question
{
    public class QuestionRetrievalFailedException : Exception
    {
        public string message;

        public QuestionRetrievalFailedException()
        {
            this.message = "An error occurred while retrieving the question.";
        }
        public QuestionRetrievalFailedException(string message)
        {
          
            this.message = message;
        }

        public override string Message => message;
    }
}
