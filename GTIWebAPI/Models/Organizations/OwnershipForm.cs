namespace GTIWebAPI.Models.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OwnershipForm")]
    public partial class OwnershipForm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OwnershipForm()
        {
            Organization = new HashSet<Organization>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string RussianName { get; set; }

        [StringLength(50)]
        public string UkrainianName { get; set; }

        [StringLength(50)]
        public string EnglishName { get; set; }

        [StringLength(250)]
        public string RussianExplanation { get; set; }

        [StringLength(250)]
        public string UkrainianExplanation { get; set; }

        [StringLength(250)]
        public string EnglishExplanation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Organization> Organization { get; set; }

        public OwnershipFormDTO ToDTO()
        {
            OwnershipFormDTO dto = new OwnershipFormDTO();
            return dto;
        }

        
    }
}
