namespace GTIWebAPI.Models.Dictionary
{
    using Employees;
    using Organizations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Languages that employee can have
    /// </summary>
    [Table("Language")]
    public partial class Language 
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Language()
        {
            EmployeeLanguage = new HashSet<EmployeeLanguage>();
            OrganizationLanguageNames = new HashSet<OrganizationLanguageName>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeLanguage> EmployeeLanguage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationLanguageName> OrganizationLanguageNames { get; set; }

        public LanguageDTO ToDTO()
        {
            LanguageDTO dto = new LanguageDTO()
            {
                Id = this.Id,
                Name = this.Name
            };
            return dto;
        }
    }


    public class LanguageDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

}
