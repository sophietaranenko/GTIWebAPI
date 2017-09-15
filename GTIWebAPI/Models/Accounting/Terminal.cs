namespace GTIWebAPI.Models.Accounting
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sklad")]
    public partial class Terminal
    {
        [Key]
        [StringLength(10)]
        [Column("kod")]
        public string Id { get; set; }

        [Required]
        [StringLength(40)]
        [Column("naimen")]
        public string Name { get; set; }

        [Column("office")]
        public int OfficeId { get; set; }

    }
}
