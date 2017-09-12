using GTIWebAPI.Models.Sales;
using GTIWebAPI.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Dictionary
{
    [Table("Office")]
    public class Office 
    {
        public Office()
        {
            Masks = new HashSet<UserRightMask>();
            UserRights = new HashSet<UserRightOff>();
            InteractionsSucceed = new HashSet<InteractionSucceed>();
        }

        public int Id { get; set; }

        public string NativeName { get; set; }

        public string ShortName { get; set; }

        public string EnglishName { get; set; }

        public string DealIndex { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<UserRightMask> Masks { get; set; }

        public virtual ICollection<UserRightOff> UserRights { get; set; }

        public virtual ICollection<InteractionSucceed> InteractionsSucceed { get; set; }

        public OfficeDTO ToDTO()
        {
            OfficeDTO dto = new OfficeDTO
            {
                Id = this.Id,
                ShortName = this.ShortName, 
                Country = this.Country == null ? null : Country.ToDTO()
            };
            return dto;
        }
    }

    public class OfficeDTO : IEquatable<OfficeDTO>
    {
        public int Id { get; set; }

        public string ShortName { get; set; }

        public string DealIndex { get; set; }

        public CountryDTO Country { get; set; }

        public Office FromDTO()
        {
            return new Office()
            {
                Id = this.Id,
                ShortName = this.ShortName
            };
        }

        public bool Equals(OfficeDTO other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }


}
