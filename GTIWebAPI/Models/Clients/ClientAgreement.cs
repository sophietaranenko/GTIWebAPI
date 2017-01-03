namespace GTIWebAPI.Models.Clients
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClientAgreement")]
    public partial class ClientAgreement : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? ClientId { get; set; }

        [StringLength(5)]
        public string Seria { get; set; }

        public int? Number { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateCreate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateBegin { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateEnd { get; set; }

        public bool? Deleted { get; set; }

        public virtual Client Client { get; set; }

        protected override string TableName
        {
            get
            {
                return "ClientAgreement";
            }
        }
    }
}
