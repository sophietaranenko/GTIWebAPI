namespace GTIWebAPI.Models.Clients
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClientTaxInfo")]
    public partial class ClientTaxInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? ClientId { get; set; }

        [StringLength(50)]
        public string TaxNo { get; set; }

        [StringLength(50)]
        public string SertificatesNo { get; set; }

        public int? TaxAddressId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateBeg { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateEnd { get; set; }

        public bool? Deleted { get; set; }

        public virtual Client Client { get; set; }
    }
}
