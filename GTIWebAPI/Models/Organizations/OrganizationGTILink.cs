namespace GTIWebAPI.Models.Organizations
{
    using Employees;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationGTILink")]
    public partial class OrganizationGTILink : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? GTIId { get; set; }

        public int? OrganizationId { get; set; }

        public bool? Deleted { get; set; }

        public int? EmployeeId { get; set; }

        public virtual Organization Organization { get; set; }

        [NotMapped]
        public virtual OrganizationGTI OrganizationGTI { get; set; }

        //[NotMapped]
        //public virtual Employee Employee { get; set; }

        protected override string TableName
        {
            get
            {
                return "OrganizationGTILink";
            }
        }

        public OrganizationGTILinkDTO ToDTO()
        {
            OrganizationGTILinkDTO dto = new OrganizationGTILinkDTO
            {
                Id = this.Id,
                GTIId = this.GTIId,
                OrganizationId = this.OrganizationId,
                OrganizationGTI = this.OrganizationGTI == null ? null : this.OrganizationGTI.ToDTO()
            };
            return dto;
        }

    }
}
