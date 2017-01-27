namespace GTIWebAPI.Models.Organizations
{
    using Dictionary;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationAddress")]
    public partial class OrganizationAddress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? OrganizationAddressTypeId { get; set; }

        public int? AddressId { get; set; }

        public bool? Deleted { get; set; }

        public virtual Address Address { get; set; } 

        public virtual Organization Organization { get; set; }

        public virtual OrganizationAddressType OrganizationAddressType { get; set; }

        public OrganizationAddressDTO ToDTO()
        {
            OrganizationAddressDTO dto = new OrganizationAddressDTO
            {
                Address = this.Address == null ? null : this.Address.ToDTO(),
                AddressId = this.AddressId,
                Id = this.Id,
                OrganizationAddressType = this.OrganizationAddressType == null ? null : this.OrganizationAddressType.ToDTO(),
                OrganizationAddressTypeId = this.OrganizationAddressTypeId,
                OrganizationId = this.OrganizationId
            };
            return dto;
        }
    }
}
