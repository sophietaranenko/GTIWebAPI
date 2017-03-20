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
    public class UserImage : GTITable
    {
        public int Id { get; set; }

        public bool IsProfilePicture { get; set; }

        public DateTime UploadDate { get; set; }

        public string UserId { get; set; }

        public string ImageName { get; set; }

        protected override string TableName
        {
            get
            {
                return "AspNetUserImages";
            }
        }
    }
}
