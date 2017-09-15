namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Quiz")]
    public class Quiz
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public bool? IsForEmployee { get; set; }

        public virtual ICollection<QuizQuestion> Questions { get; set; }

        public virtual ICollection<QuizPassing> Passings { get; set; }

        public Quiz()
        {
            Questions = new HashSet<QuizQuestion>();
            Passings = new HashSet<QuizPassing>();
        }

        public QuizDTO ToDTO()
        {
            QuizDTO dto = new QuizDTO()
            {

            };
            return dto;
        }
    }

    public class QuizDTO
    {

    }
}