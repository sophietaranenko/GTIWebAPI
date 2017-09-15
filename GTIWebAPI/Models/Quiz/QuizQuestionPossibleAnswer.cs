namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("QuizQuestionPossibleAnswer")]
    public class QuizQuestionPossibleAnswer
    {
        public int Id { get; set; }

        public int? QuizQuestionId { get; set; }

        [StringLength(1000)]
        public string AnswerText { get; set; }

        public bool? Correct { get; set; }

        public virtual QuizQuestion Question { get; set; }

        public QuizQuestionPossibleAnswerDTO ToDTO()
        {
            QuizQuestionPossibleAnswerDTO dto = new QuizQuestionPossibleAnswerDTO()
            {
                Id = this.Id,
                QuizQuestionId = this.QuizQuestionId,
                AnswerText = this.AnswerText,
                Correct = this.Correct
            };
            return dto;
        }
    }

    public class QuizQuestionPossibleAnswerDTO
    {
        public int Id { get; set; }

        public int? QuizQuestionId { get; set; }

        public string AnswerText { get; set; }

        public bool? Correct { get; set; }
    }
}