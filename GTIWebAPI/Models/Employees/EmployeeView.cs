namespace GTIWebAPI.Models.Employees
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeView")]
    public partial class EmployeeView
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(20)]
        public string IdentityCodeChar { get; set; }

        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string SecondName { get; set; }

        [StringLength(20)]
        public string Surname { get; set; }

        public string ShortAddress { get; set; }

        public int? AgeCount { get; set; }
        public string UserName { get; set; }
        public string Position { get; set; }

        [NotMapped]
        public Age Age
        {
            get
            {
                return new Age(AgeCount);
            }
        }
        [NotMapped]
        public IEnumerable<String> PositionLines
            {
            get
            {
                string[] mas;
                //удаляем последний символ |, чтобы внутри массива PositionLines не было пустых строк
                Position.Substring(Position.Length - 2, 1);
                if (Position != null && Position != "")
                {
                    mas = Position.Split('|');
                }
                else
                {
                    mas = new string[0];
                }
                return mas;
            }
        }
    }
}
