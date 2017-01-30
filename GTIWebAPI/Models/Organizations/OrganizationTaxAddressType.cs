namespace GTIWebAPI.Models.Organizations
{
    using Dictionary;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationTaxAddressType")]
    public partial class OrganizationTaxAddressType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}
