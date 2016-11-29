namespace GTIWebAPI.Models.Accounting
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("booking_incoterms")]
    public partial class Incoterms
    {
        [Key]
        [Column(Order = 0)]
        public Guid rc { get; set; }

        [Key]
        [Column("id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("nameTerm")]
        [StringLength(5)]
        public string Name { get; set; }
    }
}
