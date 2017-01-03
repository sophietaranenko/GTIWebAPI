namespace GTIWebAPI.Models.Clients
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClientSigner")]
    public partial class ClientSigner : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? ClientId { get; set; }

        [StringLength(100)]
        public string UkrNomCase { get; set; }

        [StringLength(100)]
        public string UkrGenCase { get; set; }

        [StringLength(100)]
        public string RusNomCase { get; set; }

        [StringLength(100)]
        public string RusGenCase { get; set; }

        [StringLength(100)]
        public string EngNomCase { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public int? SignerPositionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateBeg { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateEnd { get; set; }

        public bool? Deleted { get; set; }

        public virtual Client Client { get; set; }

        public virtual SignerPosition SignerPosition { get; set; }

        protected override string TableName
        {
            get
            {
                return "ClientSigner";
            }
        }
    }
}
