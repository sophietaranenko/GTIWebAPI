namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("QuizQuestionType")]
    public class QuizQuestionType
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<QuizQuestion> Questions { get; set; }

        public QuizQuestionType()
        {
            Questions = new HashSet<QuizQuestion>();
        }

        public QuizQuestionTypeDTO ToDTO()
        {
            QuizQuestionTypeDTO dto = new QuizQuestionTypeDTO()
            {
                Id = this.Id,
                Name = this.Name
            };
            return dto;
        }
    }

    public class QuizQuestionTypeDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}