namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("QuizAnswerOpen")]
    public class QuizAnswerOpen
    {
        public int Id { get; set; }

        public int? QuizPassingQuestionId { get; set; }

        [StringLength(1000)]
        public string AnswerText { get; set; }

        public QuizPassingQuestion Question { get; set; }

        public QuizAnswerOpenDTO ToDTO()
        {
            QuizAnswerOpenDTO dto = new QuizAnswerOpenDTO()
            {
                Id = this.Id,
                QuizPassingQuestionId = this.QuizPassingQuestionId,
                AnswerText = this.AnswerText
            };
            return dto;
        }
    }

    public class QuizAnswerOpenDTO
    {
        public int Id { get; set; }

        public int? QuizPassingQuestionId { get; set; }

        public string AnswerText { get; set; }
    }
}