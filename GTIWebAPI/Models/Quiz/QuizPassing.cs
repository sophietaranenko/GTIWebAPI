namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using GTIWebAPI.Models.Employees;
    using GTIWebAPI.Models.Organizations;

    [Table("QuizPassing")]
    public class QuizPassing
    {
        public int Id { get; set; }

        public DateTime FormationDate { get; set; }

        public DateTime PassingTime { get; set; }

        public int? QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<QuizPassingQuestion> Questions { get; set; }

        public virtual ICollection<QuizPassingEmployeeLink> Employees { get; set; }

        public virtual ICollection<QuizPassingOrganizationContactPersonLink> ContactPersons { get; set; }

        public QuizPassing()
        {
            Questions = new HashSet<QuizPassingQuestion>();
            Employees = new HashSet<QuizPassingEmployeeLink>();
            ContactPersons = new HashSet<QuizPassingOrganizationContactPersonLink>();
        }

        public QuizPassingDTO ToDTO()
        {
            QuizPassingDTO dto = new QuizPassingDTO()
            {
                Id = this.Id,
                FormationDate = this.FormationDate,
                PassingTime = this.PassingTime,
                QuizId = this.QuizId,
                Questions = this.Questions == null ? null : this.Questions.Select(d => d.ToDTO()).ToList()
            };
            return dto;
        }
    }

    public class QuizPassingDTO
    {
        public int Id { get; set; }

        public DateTime FormationDate { get; set; }

        public DateTime PassingTime { get; set; }

        public int? QuizId { get; set; }

        public virtual IEnumerable<QuizPassingQuestionDTO> Questions { get; set; }
    }
}