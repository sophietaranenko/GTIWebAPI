namespace GTIWebAPI.Models.Organizations
{
    using Dictionary;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationPropertyType")]
    public partial class OrganizationPropertyType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrganizationPropertyType()
        {
            OrganizationProperty = new HashSet<OrganizationProperty>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public bool? Constant { get; set; }

        public int? CountryId { get; set; }

        public int? OrganizationPropertyTypeAliasId { get; set; }

        public virtual Country Country { get; set; }

        public virtual OrganizationPropertyTypeAlias OrganizationPropertyTypeAlias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationProperty> OrganizationProperty { get; set; }

        public OrganizationPropertyTypeDTO ToDTO()
        {
            OrganizationPropertyTypeDTO dto = new OrganizationPropertyTypeDTO
            {
                Constant = this.Constant,
                Country = this.Country == null? null : this.Country.ToDTO(),
                OrganizationPropertyTypeAlias = this.OrganizationPropertyTypeAlias == null ? null : this.OrganizationPropertyTypeAlias.ToDTO(),
                OrganizationPropertyTypeAliasId = this.OrganizationPropertyTypeAliasId,
                CountryId = this.CountryId,
                Id = this.Id,
                Name = this.Name,
                Type = this.Type
            };
            return dto;
        }
    }

    public class OrganizationPropertyTypeDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool? Constant { get; set; }

        public int? CountryId { get; set; }

        public int? OrganizationPropertyTypeAliasId { get; set; }

        public CountryDTO Country { get; set; }

        public OrganizationPropertyTypeAliasDTO OrganizationPropertyTypeAlias { get; set; }

    }
}
