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
        /// <summary>
        /// Employee Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Employee date of birth
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Employee identity code
        /// </summary>
        [StringLength(20)]
        public string IdentityCode { get; set; }

        /// <summary>
        /// First name from last issued passport (EmployeePassport table)
        /// </summary>
        [StringLength(20)]
        public string FirstName { get; set; }

        /// <summary>
        /// Second name from last issued passport (EmployeePassport table)
        /// </summary>
        [StringLength(20)]
        public string SecondName { get; set; }

        /// <summary>
        /// Surname from last issued passport  (EmployeePassport table)
        /// </summary>
        [StringLength(20)]
        public string Surname { get; set; }

        /// <summary>
        /// Short Employee Address of permanent residense (from Address table, by AddressId) in format Country, City
        /// </summary>
        public string ShortAddress { get; set; }

        /// <summary>
        /// Employee Age in int
        /// </summary>
        public int? AgeCount { get; set; }

        /// <summary>
        /// UserName from AspNetUsers  
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Employees positions separated by symbol '|'
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Age object of employees ages (for Russian language) "23 ����", "21 ���", "25 ���" 
        /// </summary>
        [NotMapped]
        public Age Age
        {
            get
            {
                return new Age(AgeCount == null? 0 : AgeCount);
            }
        }

        /// <summary>
        /// Position in array of string
        /// </summary>
        [NotMapped]
        public IEnumerable<String> PositionLines
        {
            get
            {
                IEnumerable<string> mas;
                //������� ��������� ������ |, ����� ������ ������� PositionLines �� ���� ������ �����
                
                if (Position != null)
                {
                    if (Position != "")
                    {
                        Position.Substring(Position.Length - 2, 1);
                        mas = Position.Split('|');
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
                UserName = this.UserName
            };
            return dto;
        }

    }

    public class EmployeeViewDTO
    {
        /// <summary>
        /// Employee Id
        /// Id ����������
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Employee date of birth
        /// ���� �������� ����������
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Employee government identity code 
        /// ����������������� ��� ����������
        /// </summary>

        public string IdentityCode { get; set; }
        /// <summary>
        /// First name from last issued passport
        /// ��� �� ���������� �� ���� ������ ��������
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Second name from last issued passport
        /// �������� �� ���������� �� ���� ������ ��������
        /// </summary>

        public string SecondName { get; set; }
        /// <summary>
        /// Surname from last issued passport
        /// ������� �� ���������� �� ���� ������ ��������
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Short address of permanent residence in format of Country, City
        /// ������� ����� ���������� � ������� ������, �����
        /// </summary>

        public string ShortAddress { get; set; }
        /// <summary>
        /// Employee's age in int counted from date of birth
        /// ������� ����������, ����������� �� ���� �������
        /// </summary>
        public int? AgeCount { get; set; }
        /// <summary>
        /// Username of AspNetUsers connected to Employee
        /// ��� ������ ���������� �� ����� � AspNetUsers
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Today positions of employee in format Position Department Office, separated by symbol '|'
        /// ������� ��������� ���������� � ������� ��������� ����� ����, ���������� ���� �� ����� �������� '|'
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// Employee's age in string for features of Russian language
        /// ������� ���������� � ��������� �������, ��� �������� "25 ���", "23 ����" ��� "21 ���" 
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// Collection of Positions of employee in format Position Department Office
        /// ��������� �����, ������ �� ������� �������� � ���� ������ ���� ��������� � ������� ��������� ����� ���� �� ���� ���������� ����������
        /// </summary>
        public IEnumerable<String> PositionLines { get; set; }

    }
}
