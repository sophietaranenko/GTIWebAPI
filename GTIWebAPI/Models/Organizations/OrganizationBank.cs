namespace GTIWebAPI.Models.Clients
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClientBank")]
    public partial class OrganizationBank : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? ClientId { get; set; }

        public int? BankId { get; set; }

        public bool? Deleted { get; set; }

        public virtual Organization Client { get; set; }
        protected override string TableName
        {
            get
            {
                return "ClientBank";
            }
        }
    }
}
