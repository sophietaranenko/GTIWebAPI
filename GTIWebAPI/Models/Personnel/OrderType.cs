namespace GTIWebAPI.Models.Personnel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderType")]
    public partial class OrderType 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int? OrderGroupId { get; set; }

        public bool? Deleted { get; set; }

        public virtual OrderGroup OrderGroup { get; set; }
    }
}
