namespace GTIWebAPI.Models.Clients
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("ClientFile")]
    public partial class ClientFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? ClientId { get; set; }

        public int? TableId { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        [Column(TypeName = "image")]
        public byte[] FileContent { get; set; }

        [StringLength(6)]
        public string FileType { get; set; }

        [StringLength(250)]
        public string FileName { get; set; }

        public bool? Deleted { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
    }
}
