namespace GTIWebAPI.Models.Employees
{
    using Personnel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeFoundationDoc")]
    public partial class EmployeeFoundationDoc
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(5)]
        public string Seria { get; set; }

        public int? Number { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? EmployeeId { get; set; }
        [Column("FoundationDocId")]
        public int? FoundationDocumentId { get; set; }

        public bool? Deleted { get; set; }
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]

        public DateTime? IssuedWhen { get; set; }
        public string IssuedBy { get; set; }

        public virtual FoundationDocument FoundationDocument { get; set; }
    }
}
