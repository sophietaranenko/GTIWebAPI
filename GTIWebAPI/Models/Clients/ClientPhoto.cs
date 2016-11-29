using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Clients
{
    [Table("ClientPhoto")]
    public partial class ClientPhoto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int? ClientId { get; set; }
        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        [StringLength(50)]
        public string PhotoName { get; set; }
        public bool? Deleted { get; set; }
        public bool? ProfilePicture { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
    }
}