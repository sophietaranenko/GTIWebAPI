namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("QuizAnswerPossible")]
    public class QuizAnswerPossible
    {
        public int Id { get; set; }

        public int? QuizPassingQuestionId { get; set; }

        public int? QuizQuestionPossibleAnswerId { get; set; }

        public QuizPassingQuestion Question { get; set; }

        public QuizAnswerPossibleDTO ToDTO()
        {
            QuizAnswerPossibleDTO dto = new QuizAnswerPossibleDTO()
            {
                Id = this.Id,
                QuizPassingQuestionId = this.QuizPassingQuestionId,
                QuizQuestionPossibleAnswerId = this.QuizQuestionPossibleAnswerId
            };
            return dto;
        }
    }

    public class QuizAnswerPossibleDTO
    {
        public int Id { get; set; }

        public int? QuizPassingQuestionId { get; set; }

        public int? QuizQuestionPossibleAnswerId { get; set; }
    }
}