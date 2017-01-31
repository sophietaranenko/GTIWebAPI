namespace GTIWebAPI.Models.Organizations
{
    using Dictionary;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationLegalForm")]
    public partial class OrganizationLegalForm : GTITable 
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrganizationLegalForm()
        {
            Organization = new HashSet<Organization>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }


        [StringLength(250)]
        public string Explanation { get; set; }

        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Organization> Organization { get; set; }

        public OrganizationLegalFormDTO ToDTO()
        {
            OrganizationLegalFormDTO dto = new OrganizationLegalFormDTO
            {
                Explanation = this.Explanation,
                Id = this.Id,
                Name = this.Name
            };
            return dto;
        }


        protected override string TableName
        {
            get
            {
                return "OrganizationLegalForm";
            }
        }


    }
}
