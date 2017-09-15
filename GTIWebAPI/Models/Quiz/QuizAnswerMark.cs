namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("QuizAnswerMark")]
    public class QuizAnswerMark
    {
        public int Id { get; set; }

        public int? QuizPassingQuestionId { get; set; }

        public int? Mark { get; set; }

        public QuizPassingQuestion Question { get; set; }

        public QuizAnswerMarkDTO ToDTO()
        {
            QuizAnswerMarkDTO dto = new QuizAnswerMarkDTO()
            {
                Id = this.Id,
                QuizPassingQuestionId = this.QuizPassingQuestionId,
                Mark = this.Mark
            };
            return dto;
        }
    }

    public class QuizAnswerMarkDTO
    {
        public int Id { get; set; }

        public int? QuizPassingQuestionId { get; set; }

        public int? Mark { get; set; }
    }
}