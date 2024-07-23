using System.Reflection;

namespace TutorialApp.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }

        public string CourseImageUrl { get; set; }

        public Category Category { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Quiz> Quizzes { get; set; }
    }
}
