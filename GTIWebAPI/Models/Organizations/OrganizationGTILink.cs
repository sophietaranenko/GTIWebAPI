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
                OrganizationGTI = this.OrganizationGTI == null ? null : this.OrganizationGTI.ToDTO(),
                EmployeeId = this.EmployeeId
            };
            return dto;
        }

    }


    public class OrganizationGTILinkDTO
    {
        public int Id { get; set; }

        public int? GTIId { get; set; }

        public int? OrganizationId { get; set; }

        public int? EmployeeId { get; set; }

        public OrganizationGTIDTO OrganizationGTI { get; set; }

        public OrganizationGTILink FromDTO()
        {
            return new OrganizationGTILink()
            {
                EmployeeId = this.EmployeeId,
                GTIId = this.GTIId,
                Id = this.Id,
                OrganizationId = this.OrganizationId
            };
        }

    }


    public class OrganizationGTIShortDTO
    {
        public int OrganizationGTIId { get; set; }

        public int OrganizationGTIOfficeId { get; set; }

        public string OrganizationGTIOfficeShortName { get; set; }

        public string OrganizationGTIEnglishName { get; set; }

        public string CreatorShortName { get; set; }

        public int? CreatorId { get; set; }
    }

    public class OrganizationGTICreateLinkDTO
    {
        public int OrganizationId { get; set; }

        public IEnumerable<int> OrganizationGTIIds { get; set; }
    }
}
