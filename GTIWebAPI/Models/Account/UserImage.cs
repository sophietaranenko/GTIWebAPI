using GTIWebAPI.Models.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Account
{
    [Table("AspNetUserImages")]
    public class UserImage 
    {
        public Guid Id { get; set; }

        public bool? IsProfilePicture { get; set; }

        public DateTime UploadDate { get; set; }

        public string UserId { get; set; }

        public string ImageName { get; set; }

    }

    public class UserImageByteArray
    {
        public byte[] ImageContent { get; set; }

        public string FileName { get; set; }
    }
}
