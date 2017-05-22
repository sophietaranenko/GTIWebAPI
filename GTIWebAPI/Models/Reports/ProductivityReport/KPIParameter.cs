namespace GTIWebAPI.Models.Reports.ProductivityReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KPIParameter")]
    public partial class KPIParameter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KPIParameter()
        {
            KPIValue = new HashSet<KPIValue>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KPIValue> KPIValue { get; set; }

        public KPIParameterDTO ToDTO()
        {
            return new KPIParameterDTO()
            {
                Id = this.Id,
                Name = this.Name
            };
        }
    }

    public class KPIParameterDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
