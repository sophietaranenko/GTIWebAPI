namespace GTIWebAPI.Models.Accounting
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web.Mvc;

    [Table("booking")]
    public partial class Deal
    {
        [Key]
        [Column("rc")]
        public Guid Id { get; set; }

        [Column("nomer")]
        public int Number { get; set; }

        [Column("office")]
        public int OfficeId { get; set; }

        [Column("data")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [Column("cargo")]
        [StringLength(500)]
        public string Cargo { get; set; }

        [Column("vid")]
        public int DirectionId { get; set; }

        [Column("sotrud")]
        public int ManagerId { get; set; }

        [Column("sotrud2")]
        public int AssistantId { get; set; }

        [Column("status")]
        public int StatusId { get; set; }

        [Column("tip")]
        public int TypeOfDealId { get; set; }

        [Required]
        [StringLength(20)]
        [Column("poo")]
        public string PortOfOrigin { get; set; }

        [Required]
        [StringLength(20)]
        [Column("pol")]
        public string PortOfLoading { get; set; }

        [Required]
        [StringLength(20)]
        [Column("pod")]
        public string PortOfDestination { get; set; }

        [Required]
        [StringLength(20)]
        [Column("fd")]
        public string FinalDestination { get; set; }

        [Column("klient")]
        public int ClientId { get; set; }

        [Column("forwarder")]
        public int ForwarderId { get; set; }

        [Column("line")]
        public int ShippingLineId { get; set; }

        [Column("voyage")]
        public int FeederVesselId { get; set; }

        [Column("voyage_ocean")]
        public int OceanVesselId { get; set; }

        [Column("incoterms")]
        public int? IncotermId { get; set; }

        [Column("payer")]
        public int PayerId { get; set; }
        [NotMapped]
        public string POT {
            get
            {
                string result = (Direction)DirectionId == Direction.Import
                             || (Direction)DirectionId == Direction.Other
                             || (Direction)DirectionId == Direction.ImportTransit ?
                               PortOfLoading.Trim() + " / " + FinalDestination.Trim()
                               : "";
                return result;
            }
        }

        public SelectList GetStatus()
        {
            var statusList = Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return new SelectList(statusList, "Value", "Text");
        }

        public SelectList GetDirection()
        {
            var directionList = Enum.GetValues(typeof(Direction)).Cast<Direction>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            return new SelectList(directionList, "Value", "Text");
        }
    }
}
