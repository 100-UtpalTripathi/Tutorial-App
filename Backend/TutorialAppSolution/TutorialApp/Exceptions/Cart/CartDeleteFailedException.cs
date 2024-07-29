namespace TutorialApp.Exceptions.Cart
{
    public class CartDeleteFailedException : Exception
    {
        string message;

        public CartDeleteFailedException()
        {
            message = "Failed to delete item from cart.";
        }

        public CartDeleteFailedException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
        
    }
}
