namespace GTIWebAPI.Models.Clients
{
    using Dictionary;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClientTaxInfo")]
    public partial class OrganizationTaxInfo : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string TaxNo { get; set; }

        [StringLength(50)]
        public string SertificatesNo { get; set; }

        [Column("TaxAddressId")]
        public int? AddressId { get; set; }

        public virtual Address Address { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateBeg { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateEnd { get; set; }

        public bool? Deleted { get; set; }

        public virtual Organization Client { get; set; }

        public int? ClientId { get; set; }

        protected override string TableName
        {
            get
            {
                return "ClientTaxInfo";
            }
        }
    }
}
