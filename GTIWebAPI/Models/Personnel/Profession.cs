namespace GTIWebAPI.Models.Personnel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("profession")]
    public partial class Profession 
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(250)]
        [Column("naimen")]
        public string Name { get; set; }

        [StringLength(10)]
        [Column("code1")]
        public string Code { get; set; }

        [StringLength(10)]
        public string code2 { get; set; }

        public int? code3 { get; set; }

        public int? code4 { get; set; }

        public bool? Deleted { get; set; }

        public ProfessionDTO ToDTO()
        {
            ProfessionDTO dto = new ProfessionDTO
            {
                Id = this.Id,
                Name = this.Name
            };
            return dto;
        }

    }

    public class ProfessionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
