namespace TutorialApp.Exceptions.Wishlist
{
    public class UnableToRemoveFromWishlistException : Exception
    {

        public string message;

        public UnableToRemoveFromWishlistException()
        {
            message = "Unable to remove from wishlist.";
        }
        public UnableToRemoveFromWishlistException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
