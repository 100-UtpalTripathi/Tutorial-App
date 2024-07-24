namespace TutorialApp.Exceptions.Module
{
    public class NoSuchModuleFoundException : Exception
    {
        string message;

        public NoSuchModuleFoundException()
        {
            this.message = "No such module found";
        }
        public NoSuchModuleFoundException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
