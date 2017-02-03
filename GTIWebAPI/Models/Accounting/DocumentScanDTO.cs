using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Accounting
{
    public class DocumentScanDTO
    {
        public Guid Id { get; set; }

        public byte[] FileContent { get; set; }

        public string FileName { get; set; }

        public int? TableId { get; set; }

        public string TableName { get; set; }

        public int? OfficeId { get; set; }

        public string UserName { get; set; }

        public string ComputerName { get; set; }

        public DateTime? UploadDate { get; set; }

        public int? DocumentScanTypeId { get; set; }

        public DocumentScanTypeDTO DocumentScanType { get; set; }

    }
}
