using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace GTIWebAPI.Models.Accounting
{
    public class DocumentScanDTO
    {
        public Guid Id { get; set; }

        [NotMapped]
        public string Format
        {
            get
            {
                if (FileName != null && FileName.Length > 3)
                {
                    int index = FileName.LastIndexOf('.');
                    return FileName.Substring(index + 1).Trim().ToLower();
                }
                else
                {
                    return "";
                }
            }
        }

        public byte[] FileContent { get; set; }

        public string FileName { get; set; }

        public Guid TableId { get; set; }

        public string TableName { get; set; }

        public int? OfficeId { get; set; }

        public string UserName { get; set; }

        public string ComputerName { get; set; }

        public DateTime? UploadDate { get; set; }

        public Int16? DocumentScanTypeId { get; set; }

        public string DocumentScanTypeName { get; set; }

        public DocumentScanTypeDTO DocumentScanType { get; set; }

       

    }
}
