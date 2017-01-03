namespace GTIWebAPI.Models.Clients
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationType")]
    public partial class OrganizationType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrganizationType()
        {
            Client = new HashSet<Client>();
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
        public virtual ICollection<Client> Client { get; set; }
    }
}
