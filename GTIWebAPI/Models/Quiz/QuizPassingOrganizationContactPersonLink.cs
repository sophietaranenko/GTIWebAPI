namespace GTIWebAPI.Models.Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using GTIWebAPI.Models.Organizations;

    [Table("QuizPassingOrganizationContactPersonLink")]
    public class QuizPassingOrganizationContactPersonLink
    {
        public int Id { get; set; }

        public int? PassingId { get; set; }

        public int? OrganizationContactPersonId { get; set; }

        public virtual OrganizationContactPerson ContactPerson { get; set; }

        public virtual QuizPassing Passing { get; set; }
    }
}