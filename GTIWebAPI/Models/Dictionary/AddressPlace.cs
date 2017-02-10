using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Dictionary
{
    [Table("AddressPlace")]
    public partial class AddressPlace
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AddressPlace()
        {
            Addresses = new HashSet<Address>();
        }

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }

        public AddressPlaceDTO ToDTO()
        {
            AddressPlaceDTO dto = new AddressPlaceDTO()
            {
                Id = this.Id,
                Name = this.Name
            };
            return dto;
        }
    }

    public class AddressPlaceDTO
    {
        public byte Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }
    }
}
