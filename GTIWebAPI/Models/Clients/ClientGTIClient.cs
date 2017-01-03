namespace GTIWebAPI.Models.Clients
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClientGTIClient")]
    public partial class ClientGTIClient : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? GTIClientId { get; set; }

        public int? ClientId { get; set; }

        public bool? Deleted { get; set; }

        public virtual Client Client { get; set; }

        protected override string TableName
        {
            get
            {
                return "ClientGTIClient";
            }
        }
    }
}
