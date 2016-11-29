namespace GTIWebAPI.Models.Clients
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("book_cntr")]
    public partial class ClientContainer
    {
        [Column("booking")]
        public Guid DealId { get; set; }

        [Column("container")]
        [Required]
        [StringLength(12)]
        public string Name { get; set; }

        [Column("type")]
        [Required]
        [StringLength(6)]
        public string Type { get; set; }
        [Column("ves", TypeName = "numeric")]
        public decimal Weight { get; set; }

        [Column("remark")]
        [Required]
        [StringLength(500)]
        public string Remark { get; set; }

        [Column("rc")]
        [Key]
        public Guid Id { get; set; }

        [Column("office")]
        public int OfficeId { get; set; }

        [Required]
        [StringLength(40)]
        [Column("platform")]
        public string Platform { get; set; }

        [Column("vid")]
        public int Deleted { get; set; }

        [Column("sklad")]
        [StringLength(6)]
        public string TerminalId { get; set; }

        [Column("label")]
        [StringLength(25)]
        public string Seal { get; set; }

        [Column("kod_mrn")]
        [StringLength(20)]
        public string MRNCode { get; set; }


        [Column("data_shipped")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReadinessDate { get; set; }

        [Column("data_out_plan")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ETSPOL { get; set; }


        [Column("eta_pot")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ETAPOT{ get; set; }

        [Column("eta_pod")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ETAPOD { get; set; }

        [Column("document")]
        [StringLength(20)]
        public string DocumentNo { get; set; }

        [Column("data_reg")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RegistrationDate { get; set; }

        [Column("terminal_date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TerminalHandlingDate { get; set; }

        [Column("clearance_date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ClearanceDate { get; set; }

        [Column("data_out")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? GateOutFullDate { get; set; }

        [Column("ins_policy")]
        [StringLength(30)]
        public string PolicyNo { get; set; }

        [Column("ins_data")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PolicyDate { get; set; }

        [Column("data_klient")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryDate { get; set; }


        [Column("data_in")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? GateInEmptyDate { get; set; }

        //public DateTime? data_out_plan { get; set; }



        //public DateTime? data_klient { get; set; }

        //public decimal? ref_s { get; set; }

        //[StringLength(20)]
        //public string ref_d { get; set; }

        //public DateTime? data_d { get; set; }

        //public DateTime? data_plan_r { get; set; }

        //public DateTime? data_plan_in { get; set; }

        //public DateTime? data_plan_sob { get; set; }

        //public int? pack_kod { get; set; }

        //public decimal? pack_num { get; set; }

        //[StringLength(50)]
        //public string plc_customs { get; set; }

        //[StringLength(50)]
        //public string plc_discharge { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? disp_data_reg { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? disp_data_out { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? disp_data_klient { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? disp_data_in { get; set; }

        //public Guid? atp_rc { get; set; }

        //[StringLength(35)]
        //public string cmr { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? data_klient_plan { get; set; }

        //public byte? subtip { get; set; }

        //public int? service_pro { get; set; }

        //[StringLength(20)]
        //public string kod_mrn { get; set; }


        //public int? ins_klient { get; set; }

        //public DateTime? ins_data { get; set; }

        //public DateTime? dop_date5 { get; set; }

        //public int? dop_date1 { get; set; }

        //public int? dop_date2 { get; set; }

        //public int? dop_date3 { get; set; }

        //public int? dop_date4 { get; set; }

        //public DateTime? dop_date6 { get; set; }

        //public int? capacity { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal? shipping_rate { get; set; }

        //public int? tare { get; set; }

        //public int? pr_nds { get; set; }

        //public int? pr_rsvd { get; set; }

        //public int? food_chem { get; set; }

        //public DateTime? date_release_gns { get; set; }

        //[StringLength(15)]
        //public string number_release_gns { get; set; }

        //public int? importer { get; set; }

        //public int? carrier { get; set; }

        //[StringLength(50)]
        //public string gtd { get; set; }

        //public DateTime? terminal_date { get; set; }

    }
}
