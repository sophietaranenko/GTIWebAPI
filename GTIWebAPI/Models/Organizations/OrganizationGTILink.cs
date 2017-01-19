namespace GTIWebAPI.Models.Clients
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationGTILink")]
    public partial class OrganizationGTILink : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? OrganizationGTIId { get; set; }

        public int? OrganizationId { get; set; }

        public bool? Deleted { get; set; }

        public virtual Organization Organization { get; set; }

        protected override string TableName
        {
            get
            {
                return "OrganizationGTILink";
            }
        }
    }
}
