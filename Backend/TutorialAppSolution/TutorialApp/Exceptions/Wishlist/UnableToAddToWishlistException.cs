namespace TutorialApp.Exceptions.Wishlist
{
    public class UnableToAddToWishlistException : Exception
    {
        string message;

        public UnableToAddToWishlistException()
        {
            message = "Unable to add to wishlist.";
        }
        public UnableToAddToWishlistException(string message)
        {
            this.message = message;
        }

        public string Message => message;
    }
}
