namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("QuizQuestion")]
    public class QuizQuestion
    {
        public int Id { get; set; }

        public int QuizId { get; set; }

        [StringLength(1000)]
        public string QuestionText { get; set; }

        public int QuizQuestiopnTypeId { get; set; }

        public int? Number { get; set; }

        public bool? Verifiable { get; set; }

        public int? GroupId { get; set; }

        public virtual Quiz Quiz { get; set; }
        
        public virtual QuizQuestionType Type { get; set; }

        public virtual ICollection<QuizQuestion> Group { get; set; }

        public virtual ICollection<QuizQuestionPossibleAnswer> PossibleAnswers { get; set; }

        public virtual ICollection<QuizQuestionBit> BitAnswer { get; set; }

        public virtual ICollection<QuizQuestionMarkFrame> MarkFrame { get; set; }

        public virtual ICollection<QuizPassingQuestion> Passings { get; set; }

        public QuizQuestion()
        {
            Group = new HashSet<QuizQuestion>();
            PossibleAnswers = new HashSet<QuizQuestionPossibleAnswer>();
            BitAnswer = new HashSet<QuizQuestionBit>();
            MarkFrame = new HashSet<QuizQuestionMarkFrame>();
            Passings = new HashSet<QuizPassingQuestion>();
        }

        public QuizQuestionDTO ToDTO()
        {
            QuizQuestionDTO dto = new QuizQuestionDTO()
            {
                Id = this.Id,
                QuizId = this.QuizId,
                QuestionText = this.QuestionText,
                QuizQuestiopnTypeId = this.QuizQuestiopnTypeId,
                Number = this.Number,
                Verifiable = this.Verifiable,
                GroupId = this.GroupId,
                Type = this.Type == null ? null : this.Type.ToDTO(),
                Group = this.Group == null ? null : this.Group.Select(d => d.ToDTO()).ToList(),
                PossibleAnswers = this.PossibleAnswers == null ? null : this.PossibleAnswers.Select(d => d.ToDTO()).ToList(),
                MarkFrame = this.MarkFrame == null ? null : this.MarkFrame.Select(d => d.ToDTO()).ToList()
            };
            return dto;
        }
    }

    public class QuizQuestionDTO
    {
        public int Id { get; set; }

        public int QuizId { get; set; }

        public string QuestionText { get; set; }

        public int QuizQuestiopnTypeId { get; set; }

        public int? Number { get; set; }

        public bool? Verifiable { get; set; }

        public int? GroupId { get; set; }

        public QuizQuestionTypeDTO Type { get; set; }

        public IEnumerable<QuizQuestionDTO> Group { get; set; }

        public IEnumerable<QuizQuestionPossibleAnswerDTO> PossibleAnswers { get; set; }

        public IEnumerable<QuizQuestionMarkFrameDTO> MarkFrame { get; set; }
    }
}