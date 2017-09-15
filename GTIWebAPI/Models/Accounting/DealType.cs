namespace GTIWebAPI.Models.Accounting
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("book_vid")]
    public partial class DealType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("kod")]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Column("naimen")]
        public string Name { get; set; }

    }
}
