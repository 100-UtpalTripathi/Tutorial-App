using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TutorialApp.Models
{
    public class Question
    {
        [Key]
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int QuizId { get; set; }

        [Required]
        public string Text { get; set; }

        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }
        public string? OptionD { get; set; }
        public string? CorrectAnswer { get; set; }

        public Quiz Quiz { get; set; }
    }

}
