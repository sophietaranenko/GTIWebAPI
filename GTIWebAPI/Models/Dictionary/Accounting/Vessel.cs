namespace GTIWebAPI.Models.Accounting
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("voyage")]
    public partial class Vessel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("kod")]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        [Column("vessel")]
        public string VesselName { get; set; }

        [Column("data")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(10)]
        [Column("voy")]
        public string Name { get; set; }

        public override string ToString()
        {
            string result = Date.ToString().Trim() + " " + VesselName.Trim() + " V." + Name.Trim();
            return result;
        }
    }
}
