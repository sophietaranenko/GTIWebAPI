namespace GTIWebAPI.Models.Organizations
{
    using Service;
    using Personnel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationContactPersonContact")]
    public partial class OrganizationContactPersonContact : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? OrganizationContactPersonId { get; set; }

        public int? ContactTypeId { get; set; }

        public virtual ContactType ContactType { get; set; } 

        [StringLength(250)]
        public string Value { get; set; }

        public bool? Deleted { get; set; }

        public virtual OrganizationContactPerson OrganizationContactPerson { get; set; }

        public OrganizationContactPersonContactDTO ToDTO()
        {
            OrganizationContactPersonContactDTO dto = new OrganizationContactPersonContactDTO()
            {
                ContactType = this.ContactType == null ? null : this.ContactType.ToDTO(),
                ContactTypeId = this.ContactTypeId,
                Id = this.Id,
                OrganizationContactPersonId = this.OrganizationContactPersonId,
                Value = this.Value
            };
            return dto;
        }

        protected override string TableName
        {
            get
            {
                return "OrganizationContactPersonContact";
            }
        }
    }

    public class OrganizationContactPersonContactDTO
    {
        public int Id { get; set; }

        public int? OrganizationContactPersonId { get; set; }

        public int? ContactTypeId { get; set; }

        public ContactTypeDTO ContactType { get; set; }

        public string Value { get; set; }
    }
}
