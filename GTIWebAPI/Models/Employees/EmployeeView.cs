namespace GTIWebAPI.Models.Employees
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class for results of EmployeeFilter procedure, employee data for view and searching
    /// </summary>
    [Table("EmployeeView")]
    public partial class EmployeeView
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(20)]
        public string IdentityCode { get; set; }

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

        public string PhotoPath { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }

        [NotMapped]
        public Age Age
        {
            get
            {
                return new Age(AgeCount == null? 0 : AgeCount);
            }
        }

        [NotMapped]
        public IEnumerable<String> PositionLines
        {
            get
            {
                IEnumerable<string> mas;
                //удаляем последний символ |, чтобы внутри массива PositionLines не было пустых строк
                if (Position != null)
                {
                    if (Position != "")
                    {
                        string s = Position.Substring(0, Position.Length - 1);
                        mas = s.Split('|');
                    }
                    else
                    {
                        mas = new List<string>();
                    }
                }
                else
                {
                    mas = new List<string>();
                }
                return mas;
            }
        }

        [NotMapped]
        public IEnumerable<EmployeeContactDTO> EmployeeContacts { get; set; }

        public EmployeeViewDTO ToDTO()
        {
            EmployeeViewDTO dto = new EmployeeViewDTO
            {
                Id = this.Id,
                Age = this.Age == null ? null : this.Age.ToString(),
                DateOfBirth = this.DateOfBirth,
                AgeCount = this.AgeCount,
                FirstName = this.FirstName,
                IdentityCode = this.IdentityCode,
                Position = this.Position,
                PositionLines = this.PositionLines == null ? null : this.PositionLines,
                SecondName = this.SecondName,
                ShortAddress = this.ShortAddress,
                Surname = this.Surname,
                UserName = this.UserName,
                EmployeeContacts = this.EmployeeContacts == null ? null : this.EmployeeContacts,
                Email = this.Email,
                PhotoPath = this.PhotoPath,
                UserId = this.UserId 
            };
            return dto;
        }

    }

    public class EmployeeViewDTO
    {
        public int Id { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string IdentityCode { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Surname { get; set; }

        public string ShortAddress { get; set; }

        public int? AgeCount { get; set; }

        public string UserName { get; set; }

        public string Position { get; set; }

        public string Age { get; set; }

        public IEnumerable<String> PositionLines { get; set; }

        public IEnumerable<EmployeeContactDTO> EmployeeContacts { get; set; }

        public string PhotoPath { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }

    }
}
