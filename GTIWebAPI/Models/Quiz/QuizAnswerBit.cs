namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("QuizAnswerBit")]
    public class QuizAnswerBit
    {
        public int Id { get; set; }

        public int? QuizPassingQuestionId { get; set; }

        public bool? Answer { get; set; }

        public QuizPassingQuestion Question { get; set; }

        public QuizAnswerBitDTO ToDTO()
        {
            QuizAnswerBitDTO dto = new QuizAnswerBitDTO()
            {
                Id = this.Id,
                QuizPassingQuestionId = this.QuizPassingQuestionId,
                Answer = this.Answer
            };
            return dto;
        }
    }

    public class QuizAnswerBitDTO
    {
        public int Id { get; set; }

        public int? QuizPassingQuestionId { get; set; }

        public bool? Answer { get; set; }
    }
}