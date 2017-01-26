namespace GTIWebAPI.Models.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationContactPersonContact")]
    public partial class OrganizationContactPersonContact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? OrganizationContactPersonId { get; set; }

        public int? ContactTypeId { get; set; }

        [StringLength(250)]
        public string Value { get; set; }

        public virtual OrganizationContactPerson OrganizationContactPerson { get; set; }
    }
}
