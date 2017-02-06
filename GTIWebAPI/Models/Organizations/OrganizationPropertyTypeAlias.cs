using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    [Table("OrganizationPropertyTypeAlias")]
    public partial class OrganizationPropertyTypeAlias
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrganizationPropertyTypeAlias()
        {
            OrganizationPropertyTypes = new HashSet<OrganizationPropertyType>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationPropertyType> OrganizationPropertyTypes { get; set; }

        public OrganizationPropertyTypeAliasDTO ToDTO()
        {
            OrganizationPropertyTypeAliasDTO dto = new OrganizationPropertyTypeAliasDTO()
            {
                Id = this.Id,
                Name = this.Name
            };
            return dto;
        }
    }
}
