namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("QuizQuestionMarkFrame")]
    public class QuizQuestionMarkFrame
    {
        public int Id { get; set; }

        public int? QuizQuestionId { get; set; }

        public int? LowerValue { get; set; }

        public int? UpperValue { get; set; }

        public virtual QuizQuestion Question { get; set; }

        public QuizQuestionMarkFrameDTO ToDTO()
        {
            QuizQuestionMarkFrameDTO dto = new QuizQuestionMarkFrameDTO()
            {
                Id = this.Id,
                QuizQuestionId = this.QuizQuestionId,
                LowerValue = this.LowerValue,
                UpperValue = this.UpperValue
            };
            return dto;
        }
    }

    public class QuizQuestionMarkFrameDTO
    {
        public int Id { get; set; }

        public int? QuizQuestionId { get; set; }

        public int? LowerValue { get; set; }

        public int? UpperValue { get; set; }
    }
}