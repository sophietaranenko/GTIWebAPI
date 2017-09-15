namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("QuizPassingQuestion")]
    public class QuizPassingQuestion
    {
        public int Id { get; set; }

        public int? QuizPassingId { get; set; }

        public int? QuizQuestionId { get; set; }

        public QuizQuestion Question { get; set; }

        public QuizPassing Passing { get; set; }

        public virtual ICollection<QuizAnswerBit> AnswerBit { get; set; }

        public virtual ICollection<QuizAnswerMark> AnswerMark { get; set; }

        public virtual ICollection<QuizAnswerOpen> AnswerOpen { get; set; }

        public virtual ICollection<QuizAnswerPossible> AnswerPossible { get; set; }

        public QuizPassingQuestion()
        {
            AnswerBit = new HashSet<QuizAnswerBit>();
            AnswerMark = new HashSet<QuizAnswerMark>();
            AnswerOpen = new HashSet<QuizAnswerOpen>();
            AnswerPossible = new HashSet<QuizAnswerPossible>();
        }

        public QuizPassingQuestionDTO ToDTO()
        {
            QuizPassingQuestionDTO dto = new QuizPassingQuestionDTO()
            {
                Id = this.Id,
                QuizPassingId = this.QuizPassingId,
                QuizQuestionId = this.QuizQuestionId,
                AnswerBit = this.AnswerBit == null ? null : this.AnswerBit.Select(d => d.ToDTO()).ToList(),
                AnswerMark = this.AnswerMark == null ? null : this.AnswerMark.Select(d => d.ToDTO()).ToList(),
                AnswerOpen = this.AnswerOpen == null ? null : this.AnswerOpen.Select(d => d.ToDTO()).ToList(),
                AnswerPossible = this.AnswerPossible == null ? null : this.AnswerPossible.Select(d => d.ToDTO()).ToList()
            };
            return dto;
        }
    }

    public class QuizPassingQuestionDTO
    {
        public int Id { get; set; }

        public int? QuizPassingId { get; set; }

        public int? QuizQuestionId { get; set; }

        public virtual IEnumerable<QuizAnswerBitDTO> AnswerBit { get; set; }

        public virtual IEnumerable<QuizAnswerMarkDTO> AnswerMark { get; set; }

        public virtual IEnumerable<QuizAnswerOpenDTO> AnswerOpen { get; set; }

        public virtual IEnumerable<QuizAnswerPossibleDTO> AnswerPossible { get; set; }
    }
}