using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    [Table("OrganizationLanguageName")]
    public partial class OrganizationLanguageName : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? LanguageId { get; set; }

        public bool? Deleted { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string LegalFormName { get; set;}

        public string LegalFormShortName { get; set; }

        public virtual Language Language { get; set; }

        public virtual Organization Organization { get; set; }

        public OrganizationLanguageNameDTO ToDTO()
        {
            OrganizationLanguageNameDTO dto = new OrganizationLanguageNameDTO()
            {
                Id = this.Id,
                Language = this.Language == null ? null : this.Language.ToDTO(),
                LanguageId = this.LanguageId,
                OrganizationId = this.OrganizationId,
                Name = this.Name,
                ShortName = this.ShortName,
                LegalFormName = this.LegalFormName,
                LegalFormShortName = this.LegalFormShortName
            };
            return dto;
        }

        protected override string TableName
        {
            get
            {
                return "OrganizationLanguageShortName";
            }
        }
    }

    public class OrganizationLanguageNameDTO
    {
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? LanguageId { get; set; }

        public string ShortName { get; set; }

        public string LegalFormName { get; set; }

        public string LegalFormShortName { get; set; }

        public string Name { get; set; }

        public LanguageDTO Language { get; set; }
    }
}
