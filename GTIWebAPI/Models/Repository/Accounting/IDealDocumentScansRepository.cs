using GTIWebAPI.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Accounting
{
    public interface IDealDocumentScansRepository
    {
        List<DocumentScanTypeDTO> GetDocumentScanTypes();

        DocumentScanDTO GetDocumentScan(Guid id);

        List<DocumentScanDTO> GetDocumentScansByDealId(Guid dealId);

        DocumentScanDTO UpdateDealDocumentScan(DocumentScanDTO dto);

        DocumentScanDTO UploadDealDocumentScan(Guid dealId, byte[] fileContent, string fileName, string email, int documentScanTypeId);

        bool DeleteDealDocumentScan(Guid id);
    }
}
