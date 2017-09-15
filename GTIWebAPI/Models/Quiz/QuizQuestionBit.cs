namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("QuizQuestionBit")]
    public class QuizQuestionBit
    {
        public int Id { get; set; }

        public int? QuizQuestionId { get; set; }

        public bool? Correct { get; set; }

        public virtual QuizQuestion Question { get; set; }

        public QuizQuestionBitDTO ToDTO()
        {
            QuizQuestionBitDTO dto = new QuizQuestionBitDTO()
            {
                Id = this.Id,
                QuizQuestionId = this.QuizQuestionId,
                Correct = this.Correct
            };
            return dto;
        }
    }

    public class QuizQuestionBitDTO
    {
        public int Id { get; set; }

        public int? QuizQuestionId { get; set; }

        public bool? Correct { get; set; }
    }
}