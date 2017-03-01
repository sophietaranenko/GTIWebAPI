namespace GTIWebAPI.Models.Employees
{
    using Service;
    using Personnel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeFoundationDoc")]
    public partial class EmployeeFoundationDocument : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(10)]
        public string Seria { get; set; }

        [Column("NumberChar")]
        [StringLength(25)]
        public string Number { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? EmployeeId { get; set; }

        /// <summary>
        /// Id to table FoundationDocument
        /// </summary>
        [Column("FoundationDocId")]
        public int? FoundationDocumentId { get; set; }

        public bool? Deleted { get; set; }
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]

        public DateTime? IssuedWhen { get; set; }
        [StringLength(250)]
        public string IssuedBy { get; set; }

        public virtual FoundationDocument FoundationDocument { get; set; }

        protected override string TableName
        {
            get
            {
                return "EmployeeFoundationDoc";
            }
        }

        public EmployeeFoundationDocumentDTO ToDTO()
        {
            EmployeeFoundationDocumentDTO dto = new EmployeeFoundationDocumentDTO()
            {
                FoundationDocument = this.FoundationDocument == null ? null : this.FoundationDocument.ToDTO(),
                Description =  this.Description,
                EmployeeId = this.EmployeeId,
                FoundationDocumentId = this.FoundationDocumentId,
                Id = this.Id,
                IssuedBy = this.IssuedBy,
                IssuedWhen = this.IssuedWhen,
                Number = this.Number,
                Seria = this.Seria
            };
            return dto;
        }
    }


    public class EmployeeFoundationDocumentDTO
    {
        public int Id { get; set; }

        public string Seria { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime? IssuedWhen { get; set; }

        public string IssuedBy { get; set; }
        public int? FoundationDocumentId { get; set; }

        public FoundationDocumentDTO FoundationDocument { get; set; }
    }
}
