namespace TutorialApp.Exceptions.Category
{
    public class NoSuchCategoryFoundException : Exception
    {
        string message;
        public NoSuchCategoryFoundException()
        {
            message = "No Category found with given ID!";
        }
        public NoSuchCategoryFoundException(string name)
        {
            message = name;
        }
        public override string Message => message;
    }
}
