namespace GTIWebAPI.Models.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sotrud")]
    public partial class EmployeeGTI
    {
        [Column("kod")]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Column("naimen")]
        public string Name { get; set; }

        [Required]
        [StringLength(60)]
        [Column("fio")]
        public string NativeName { get; set; }

        [Column("office")]
        public int OfficeId { get; set; }

        [Column("prava")]
        public int GTIId { get; set; }

        [Key]
        public Guid rc { get; set; }

        [Required]
        [StringLength(50)]
        [Column("email")]
        public string EmailEnd { get; set; }


        [Required]
        [StringLength(30)]
        [Column("naimen_e")]
        public string EmailBegin { get; set; }



        [NotMapped]
        public string Email
        {
            get
            {
                string result = "";
                result += EmailBegin != null ? EmailBegin.Trim() : "";
                result += EmailEnd != null ? EmailEnd.Trim() : "";
                return result;
            }
        }

        //[StringLength(40)]
        //public string dolj_rus { get; set; }

        //[StringLength(40)]
        //public string dolj_eng { get; set; }

        //public DateTime? data_uvolen { get; set; }

        //public bool booking { get; set; }

        //public int otdel { get; set; }

        //public bool eksp { get; set; }

        //public int tip { get; set; }

        //public int? chat { get; set; }

        //[StringLength(40)]
        //public string fio_eng { get; set; }

        //[StringLength(40)]
        //public string otdel_rus { get; set; }

        //[StringLength(40)]
        //public string otdel_eng { get; set; }

        //[Required]
        //[StringLength(250)]
        //public string tel { get; set; }

        //[Required]
        //[StringLength(250)]
        //public string adres { get; set; }

        //public DateTime? data_r { get; set; }

        //[Required]
        //[StringLength(250)]
        //public string prim { get; set; }

        //public int kod2 { get; set; }

        //public bool uvolen { get; set; }

        //[StringLength(20)]
        //public string tel_d { get; set; }

        //[Required]
        //[StringLength(20)]
        //public string tel_m { get; set; }

        //[Required]
        //[StringLength(250)]
        //public string living_reason { get; set; }

        //[Required]
        //[StringLength(250)]
        //public string deti { get; set; }

        //[Required]
        //[StringLength(250)]
        //public string rodstv { get; set; }

        //[Required]
        //[StringLength(250)]
        //public string hobby { get; set; }

        //[Column(TypeName = "ntext")]
        //[Required]
        //public string foto { get; set; }

        //public bool english { get; set; }

        //public bool german { get; set; }

        //public bool french { get; set; }

        //public bool spain { get; set; }

        //public bool italian { get; set; }

        //public bool chinese { get; set; }

        //[Required]
        //[StringLength(250)]
        //public string lang { get; set; }

        //public int uvolen_n { get; set; }

        //public int pol { get; set; }

        //public int jenat { get; set; }

        //public int user_kod { get; set; }

        //public int english_level { get; set; }

        //public int german_level { get; set; }

        //public int french_level { get; set; }

        //public int spain_level { get; set; }

        //public int italian_level { get; set; }

        //public int chinese_level { get; set; }

        //[Column(TypeName = "ntext")]
        //public string instruction { get; set; }

        //[Required]
        //[StringLength(20)]
        //public string foto_end { get; set; }

        //public bool pr_progr { get; set; }

        //public Guid? doljnost { get; set; }

        //[Required]
        //[StringLength(11)]
        //public string kod_ved { get; set; }

        //public DateTime? data_priema { get; set; }

        //[Required]
        //[StringLength(15)]
        //public string name1 { get; set; }

        //[Required]
        //[StringLength(15)]
        //public string name2 { get; set; }

        //[Required]
        //[StringLength(15)]
        //public string name3 { get; set; }

        //[Column(TypeName = "image")]
        //public byte[] photo { get; set; }

        //[Required]
        //[StringLength(30)]
        //public string photo_name { get; set; }

        //[StringLength(60)]
        //public string fio_ukr { get; set; }

        //public bool? notice { get; set; }

        //[StringLength(40)]
        //public string old_dolj_eng { get; set; }
    }
}
