namespace GTIWebAPI.Models.Accounting
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("line")]
    public partial class ShippingLine
    {
        [Key]
        [Column("kod")]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Column("naimen")]
        public string Name { get; set; }

    }
}
