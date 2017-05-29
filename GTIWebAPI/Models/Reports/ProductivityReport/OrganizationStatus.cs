using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Reports.ProductivityReport
{
    public class OrganizationStatus
    {
        public int OrganizationGTIId{ get; set; }

        public string OrganizationName { get; set; }
        
        public int Potential { get; set; }
        
        public int QuarantineNoDeal { get; set; }
        
        public int QuarantineWithDeal { get; set; }
        
        public int Lost { get; set; }
        
        public int Active { get; set; }
        
        public int EmptyStatus { get; set; }
        
        public int Unattended { get; set; }
         
    }

    public class OrganizationStatusDTO
    {
        public OrganizationStatusDTO()
        {
        }

        public OrganizationStatusDTO(IEnumerable<OrganizationStatus> statuses)
        {
            this.Potential = statuses.Where(d => d.Potential == 1).Select(d => new OrganizationStatusEntityDTO { Id = d.OrganizationGTIId, Name = d.OrganizationName });
            this.QuarantineNoDeals = statuses.Where(d => d.QuarantineNoDeal == 1).Select(d => new OrganizationStatusEntityDTO { Id = d.OrganizationGTIId, Name = d.OrganizationName });
            this.QuarantineWithDeals = statuses.Where(d => d.QuarantineWithDeal == 1).Select(d => new OrganizationStatusEntityDTO { Id = d.OrganizationGTIId, Name = d.OrganizationName });
            this.Lost = statuses.Where(d => d.Lost == 1).Select(d => new OrganizationStatusEntityDTO { Id = d.OrganizationGTIId, Name = d.OrganizationName });
            this.Active = statuses.Where(d => d.Active == 1).Select(d => new OrganizationStatusEntityDTO { Id = d.OrganizationGTIId, Name = d.OrganizationName });
            this.EmptyStatus = statuses.Where(d => d.EmptyStatus == 1).Select(d => new OrganizationStatusEntityDTO { Id = d.OrganizationGTIId, Name = d.OrganizationName });
            this.Unattended = statuses.Where(d => d.Unattended == 1).Select(d => new OrganizationStatusEntityDTO { Id = d.OrganizationGTIId, Name = d.OrganizationName });
        }

        public IEnumerable<OrganizationStatusEntityDTO>  Potential { get; set; }

        public IEnumerable<OrganizationStatusEntityDTO> QuarantineNoDeals { get; set; }

        public IEnumerable<OrganizationStatusEntityDTO> QuarantineWithDeals { get; set; }

        public IEnumerable<OrganizationStatusEntityDTO> Lost { get; set; }

        public IEnumerable<OrganizationStatusEntityDTO> Active { get; set; }

        public IEnumerable<OrganizationStatusEntityDTO> EmptyStatus { get; set; }

        public IEnumerable<OrganizationStatusEntityDTO> Unattended { get; set; }
    }

    public class OrganizationStatusEntityDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
