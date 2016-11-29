namespace GTIWebAPI.Models.Personnel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContactType")]
    public partial class ContactType 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Icon { get; set; }
        public bool? Deleted { get; set; }
    }
}
