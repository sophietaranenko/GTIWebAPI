namespace GTIWebAPI.Models.Reports.ProductivityReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KPIValue")]
    public partial class KPIValue
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateBegin { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateEnd { get; set; }

        public int KPIPeriodId { get; set; }

        public int KPIParameterId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Value { get; set; }

        public bool? Deleted { get; set; }

        public virtual KPIParameter KPIParameter { get; set; }

        public virtual KPIPeriod KPIPeriod { get; set; }

        public KPIValueDTO ToDTO()
        {
            return new KPIValueDTO()
            {
                Id = this.Id,
                DateBegin = this.DateBegin,
                DateEnd = this.DateEnd,
                KPIPeriodId = this.KPIPeriodId,
                KPIParameterId = this.KPIParameterId,
                KPIParameter = this.KPIParameter == null ? null : this.KPIParameter.ToDTO(),
                KPIPeriod = this.KPIPeriod == null ? null : this.KPIPeriod.ToDTO()
            };
        }
    }


    public class KPIValueDTO
    {
        public int Id { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public int KPIPeriodId { get; set; }

        public int KPIParameterId { get; set; }

        public  KPIParameterDTO KPIParameter { get; set; }

        public  KPIPeriodDTO KPIPeriod { get; set; }
    }
}
