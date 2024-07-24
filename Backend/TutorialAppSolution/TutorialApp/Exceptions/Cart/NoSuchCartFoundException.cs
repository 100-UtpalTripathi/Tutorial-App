namespace TutorialApp.Exceptions.Cart
{
    public class NoSuchCartFoundException : Exception
    {
        string message;
        public NoSuchCartFoundException()
        {
            message = "No Cart found with given ID!";
        }
        public NoSuchCartFoundException(string name)
        {
            message = name;
        }
        public override string Message => message;
    }
}