namespace GTIWebAPI.Models.Personnel
{
    using Dictionary;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("profession")]
    public partial class Profession : GTITable
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
        public string Code2 { get; set; }

        public int? code3 { get; set; }

        public int? code4 { get; set; }

        public bool? Deleted { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public ProfessionDTO ToDTO()
        {
            ProfessionDTO dto = new ProfessionDTO
            {
                Id = this.Id,
                Name = this.Name,
                Country = this.Country == null ? null : this.Country.ToDTO(),
                Code = this.Code,
                Code2 = this.Code2
            };
            return dto;
        }

        protected override string TableName
        {
            get
            {
                return "Profession";
            }
        }

    }

    public class ProfessionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int CountryId { get; set; }

        public CountryDTO Country { get; set; }

        public string Code { get; set; }

        public string Code2 { get; set; }

        public Profession FromDTO()
        {
            return new Profession()
            {
                Code = this.Code,
                Code2 = this.Code2,
                Id = this.Id,
                Name = this.Name,
                CountryId = this.CountryId
            };
        }
    }
}
