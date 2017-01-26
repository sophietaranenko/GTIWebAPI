namespace GTIWebAPI.Models.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationProperty")]
    public partial class OrganizationProperty
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? OrganizationPropertyTypeId { get; set; }

        [StringLength(250)]
        public string Value { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateBegin { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateEnd { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual OrganizationPropertyType OrganizationPropertyType { get; set; }
    }
}
