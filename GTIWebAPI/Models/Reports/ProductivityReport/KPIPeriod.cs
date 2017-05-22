namespace GTIWebAPI.Models.Reports.ProductivityReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KPIPeriod")]
    public partial class KPIPeriod
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KPIPeriod()
        {
            KPIValue = new HashSet<KPIValue>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KPIValue> KPIValue { get; set; }

        public KPIPeriodDTO ToDTO()
        {
            return new KPIPeriodDTO()
            {
                Id = this.Id,
                Name = this.Name
            };
        }
    }

    public class KPIPeriodDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
