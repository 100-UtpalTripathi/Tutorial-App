namespace Tutorial_App.Models
{
    public class Module
    {
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public Course Course { get; set; }
    }

}
