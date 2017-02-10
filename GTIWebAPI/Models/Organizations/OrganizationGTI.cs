using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    [Table("OrganizationGTI")]
    public partial class OrganizationGTI 
    {
        [Column("kod")]
        public int Id { get; set; }

        [Column("naimen_f")]
        public string EnglishName { get; set; }

        [Column("naimen_e")]
        public string NativeName { get; set; }

        [Column("naimen")]
        public string ShortName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("fax")]
        public string Fax { get; set; }

        [Column("tel")]
        public string Phone { get; set; }

        [Column("adres")]
        public string Address { get; set; }

        [Column("country")]
        public string Country { get; set; }

        [Column("kod_ident")]
        public string RegistrationNumber { get; set; }

        [Column("nomer")]
        public string TaxNumber { get; set; }

        [Column("office")]
        public int OfficeId { get; set; }

        public Office Office { get; set; }

        public OrganizationGTIDTO ToDTO()
        {
            OrganizationGTIDTO dto = new OrganizationGTIDTO()
            {
                Address = this.Address,
                Email = this.Email,
                EnglishName = this.EnglishName,
                Country = this.Country,
                Fax = this.Fax,
                NativeName = this.NativeName,
                RegistrationNumber = this.RegistrationNumber,
                TaxNumber = this.TaxNumber,
                Id = this.Id,
                Office = this.Office == null ? null : this.Office.ToDTO(),
                OfficeId = this.OfficeId,
                Phone = this.Phone,
                ShortName = this.ShortName
            };
            return dto;
        }

    }

    public class OrganizationGTIDTO
    {
        public int Id { get; set; }

        public string EnglishName { get; set; }

        public string NativeName { get; set; }

        public string ShortName { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public string RegistrationNumber { get; set; }

        public string TaxNumber { get; set; }

        public int OfficeId { get; set; }

        public OfficeDTO Office { get; set; }
    }
}
