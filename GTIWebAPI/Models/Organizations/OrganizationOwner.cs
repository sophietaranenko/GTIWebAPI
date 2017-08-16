namespace GTIWebAPI.Models.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationOwner")]
    public partial class OrganizationOwner
    {
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime? DateBegin { get; set; }
    }
}
