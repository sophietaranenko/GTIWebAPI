namespace GTIWebAPI.Models.Clients
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SignerPosition")]
    public partial class SignerPosition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SignerPosition()
        {
            ClientSigner = new HashSet<ClientSigner>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(150)]
        public string RusNomCase { get; set; }

        [StringLength(150)]
        public string RusGenCase { get; set; }

        [StringLength(150)]
        public string UkrNomCase { get; set; }

        [StringLength(150)]
        public string UkrGenCase { get; set; }

        [StringLength(150)]
        public string EngNomCase { get; set; }

        public int? OrderByColumn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientSigner> ClientSigner { get; set; }
    }
}
