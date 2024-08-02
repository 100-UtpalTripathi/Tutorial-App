﻿namespace TutorialApp.Models.DTOs.Question
{
    public class QuestionDTO
    {   
        public int QuizId { get; set; }
        public string Text { get; set; }
        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }
        public string? OptionD { get; set; }
        public string? CorrectAnswer { get; set; }

    }
}
