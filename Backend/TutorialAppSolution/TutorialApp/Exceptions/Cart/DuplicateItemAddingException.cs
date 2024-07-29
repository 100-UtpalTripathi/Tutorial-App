namespace TutorialApp.Exceptions.Cart
{
    public class DuplicateItemAddingException : Exception
    {
        string message;

        public DuplicateItemAddingException()
        {
            message = "Item already exists in cart.";
        }

        public DuplicateItemAddingException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
