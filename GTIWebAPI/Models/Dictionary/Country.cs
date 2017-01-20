using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Dictionary
{
    [Table("Country")]
    public partial class Country
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Country()
        {
            Addresses = new HashSet<Address>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Alfa2Code { get; set; }

        [StringLength(50)]
        public string Alfa3Code { get; set; }

        [StringLength(50)]
        public string IsoCode { get; set; }

        public int ContinentId { get; set; }

        [StringLength(50)]
        public string PhoneCode { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string InternationalName { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public bool? Deleted { get; set; }


        public virtual Continent Continent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
