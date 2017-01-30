namespace GTIWebAPI.Models.Organizations
{
    using Dictionary;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationTaxAddress")]
    public partial class OrganizationTaxAddress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? AddressId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateBegin { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateEnd { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual Address Address { get; set; }

        public OrganizationTaxAddressDTO ToDTO()
        {
            OrganizationTaxAddressDTO dto = new OrganizationTaxAddressDTO
            {
                DateBegin = this.DateBegin,
                DateEnd = this.DateEnd,
                Address = this.Address == null ? null : this.Address.ToDTO(),
                AddressId = this.AddressId,
                Id = this.Id,
                OrganizationId = this.OrganizationId
            };
            return dto;
        }

    }
}
