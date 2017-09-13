namespace GTIWebAPI.Models.Sales
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("Act")]
    public partial class Act
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Act()
        {
            InteractionAct = new HashSet<InteractionAct>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InteractionAct> InteractionAct { get; set; }

        public ActDTO ToDTO()
        {
            return new ActDTO()
            {
                Id = this.Id,
                Name = this.Name
            };
        }
    }

    public class ActDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Act FromDTO()
        {
            return new Act()
            {
                Id = this.Id,
                Name = this.Name
            };
        }
    }
}
