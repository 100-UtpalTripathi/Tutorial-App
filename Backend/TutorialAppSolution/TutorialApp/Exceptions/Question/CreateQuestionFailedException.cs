namespace TutorialApp.Exceptions.Question
{
    public class CreateQuestionFailedException : Exception
    {
        string message;

        public CreateQuestionFailedException()
        {
            message = "Failed to create question.";
        }
        public CreateQuestionFailedException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
