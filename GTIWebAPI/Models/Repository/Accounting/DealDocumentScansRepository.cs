using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Repository.Accounting
{
    public class DealDocumentScansRepository : IDealDocumentScansRepository
    {
        private IDbContextFactory factory;
        public DealDocumentScansRepository()
        {
            factory = new DbContextFactory();
        }


        public List<DocumentScanDTO> GetDocumentScansByDealId(Guid dealId)
        {
            List<DocumentScanDTO> dtos = new List<DocumentScanDTO>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                dtos = db.GetDocumentScanByDeal(dealId);
                List<DocumentScanTypeDTO> types = db.GetDocumentScanTypes();
                if (dtos != null && types != null)
                {
                    foreach (var item in dtos)
                    {
                        item.DocumentScanType = types.Where(d => d.Id == item.DocumentScanTypeId).FirstOrDefault();
                    }
                }
            }
            return dtos;
        }


        public DocumentScanDTO UpdateDealDocumentScan(DocumentScanDTO dto)
        {
            using (IAppDbContext db = factory.CreateDbContext())
            {
                dto = db.UpdateDocumentScanType(dto.Id, (int)dto.DocumentScanTypeId);
                if (dto != null)
                {
                    List<DocumentScanTypeDTO> types = db.GetDocumentScanTypes();
                    if (types != null)
                    {
                        dto.DocumentScanType = types.Where(d => d.Id == dto.DocumentScanTypeId).FirstOrDefault();
                    }
                }
            }
            return dto;
        }


        public DocumentScanDTO UploadDealDocumentScan(Guid dealId, byte[] fileContent, string fileName, string email, int documentScanTypeId)
        {
            DocumentScanDTO dto = new DocumentScanDTO();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                Guid scanId = db.InsertDealDocumentScan(dealId, fileContent, fileName, email, documentScanTypeId);
                dto = db.GetDealDocumentScanById(scanId);
                if (dto != null)
                {
                    List<DocumentScanTypeDTO> types = db.GetDocumentScanTypes();
                    if (types != null)
                    {
                        dto.DocumentScanType = types.Where(d => d.Id == dto.DocumentScanTypeId).FirstOrDefault();
                    }
                }
            }
            return dto;
        }


        public bool DeleteDealDocumentScan(Guid id)
        {
            bool result = false;
            using (IAppDbContext db = factory.CreateDbContext())
            {
                result = db.DeleteDocumentScan(id);
            }
            return result;
        }

        public List<DocumentScanTypeDTO> GetDocumentScanTypes()
        {
            List<DocumentScanTypeDTO> dtos = new List<DocumentScanTypeDTO>();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                dtos = db.GetDocumentScanTypes();
            }
            return dtos;
        }

        public DocumentScanDTO GetDocumentScan(Guid id)
        {
            DocumentScanDTO dto = new DocumentScanDTO();
            using (IAppDbContext db = factory.CreateDbContext())
            {
                dto = db.GetDealDocumentScanById(id);
                if (dto != null)
                {
                    List<DocumentScanTypeDTO> types = db.GetDocumentScanTypes();
                    if (types != null)
                    {
                        dto.DocumentScanType = types.Where(d => d.Id == dto.DocumentScanTypeId).FirstOrDefault();
                    }
                }
            }
            return dto;
        }
    }
}
