namespace GTIWebAPI.Models.Clients
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClientContact")]
    public partial class OrganizationContact : GTITable 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string SecondName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(250)]
        public string Position { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string PhoneAdd { get; set; }

        [StringLength(20)]
        public string PhoneHome { get; set; }

        [StringLength(30)]
        public string Skype { get; set; }

        [StringLength(30)]
        public string Email { get; set; }

        [StringLength(30)]
        public string EmailAdd { get; set; }

        public int? ClientId { get; set; }

        public bool? Deleted { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        public virtual Organization Client { get; set; }

        protected override string TableName
        {
            get
            {
                return "ClientContact";
            }
        }
    }
}
