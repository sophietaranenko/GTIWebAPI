namespace GTIWebAPI.Models.Organizations
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrganizationProperty")]
    public partial class OrganizationProperty : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? OrganizationPropertyTypeId { get; set; }

        [StringLength(250)]
        public string Value { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateBegin { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateEnd { get; set; }

        public bool? Deleted { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual OrganizationPropertyType OrganizationPropertyType { get; set; }

        public OrganizationPropertyDTO ToDTO()
        {
            OrganizationPropertyDTO dto = new OrganizationPropertyDTO()
            {
                DateBegin = this.DateBegin,
                DateEnd = this.DateEnd,
                Id = this.Id,
                OrganizationId = this.OrganizationId,
                OrganizationPropertyTypeId = this.OrganizationPropertyTypeId,
                Value = this.Value,
                OrganizationPropertyType = this.OrganizationPropertyType == null ? null : this.OrganizationPropertyType.ToDTO()
            };
            return dto;
        }

        protected override string TableName
        {
            get
            {
                return "OrganizationProperty";
            }
        }
    }

    public class OrganizationPropertyDTO
    {
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? OrganizationPropertyTypeId { get; set; }

        public string Value { get; set; }

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public OrganizationPropertyTypeDTO OrganizationPropertyType { get; set; }
    }
}
