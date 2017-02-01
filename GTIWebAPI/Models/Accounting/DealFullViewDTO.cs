using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Accounting
{
    public class DealFullViewDTO
    {
        public string FullNumber { get; set; }

        public int? Number { get; set; }

        public DateTime? CreateDate { get; set; }

        public string Direction { get; set; }

        public int? DirectionId { get; set; }

        public string Cargo { get; set; }

        public int? ShippingLineId { get; set; }

        public string ShippingLine { get; set; }

        public string POO { get; set; }

        public string POL { get; set; }

        public string POD { get; set; }

        public string FD { get; set; }

        public string Office { get; set; }

        public int? OfficeId { get; set; }

        public int? OrganizationGTIId { get; set; }

        public int? OrganizationId { get; set; }

        public int? ForwarderGTIId { get; set; }

        public int? ForwarderId { get; set; }

        public int? ManagerGTIId { get; set; }

        public int? ManagerId { get; set; }

        public string MaganerPicture { get; set; }

        public int? AssistantGTIId { get; set; }

        public int? AssistantId { get; set; }

        public string AssistantPicture { get; set; }

        public string StatusChar { get; set; }

        public int? StatusId { get; set; }

        public string DealType { get; set; }

        public int? DealTypeId { get; set; }

        public string Incoterms { get; set; }

        public int? IncotermsId { get; set; }

        public string FeederVessel { get; set; }

        public int? FeederVesselId { get; set; }

        public string OceanVessel { get; set; }

        public int? OceanVesselId { get; set; }

        public string POT { get; set; }

        public Guid Id { get; set; }

        public IEnumerable<DealInvoiceViewDTO> Invoices { get; set; }

        public IEnumerable<DealContainerViewDTO> Containers { get; set; }

    }
}
