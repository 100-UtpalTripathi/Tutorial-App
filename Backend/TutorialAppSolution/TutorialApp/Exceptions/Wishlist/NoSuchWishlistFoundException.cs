namespace TutorialApp.Exceptions.Wishlist
{
    public class NoSuchWishlistFoundException : Exception
    {
        string message;

        public NoSuchWishlistFoundException()
        {
            this.message = "No such wishlist found";
        }
        public NoSuchWishlistFoundException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
