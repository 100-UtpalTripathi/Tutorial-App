namespace TutorialApp.Models.DTOs.Cart
{
    public class CartDTO
    {
        public string UserEmail { get; set; }
        public int CourseId { get; set; }

        public decimal Price { get; set; }
    }
}
