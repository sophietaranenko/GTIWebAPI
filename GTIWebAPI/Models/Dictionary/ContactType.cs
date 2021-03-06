namespace GTIWebAPI.Models.Personnel
{
    using Employees;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContactType")]
    public partial class ContactType 
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContactType()
        {
            EmployeeContact = new HashSet<EmployeeContact>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Icon { get; set; }
        public bool? Deleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeContact> EmployeeContact { get; set; }

        public ContactTypeDTO ToDTO()
        {
            ContactTypeDTO dto = new ContactTypeDTO()
            {
                Id = this.Id,
                Name = this.Name
            };
            return dto;
        }
    }

    public class ContactTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
