using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTIWebAPI.Models.Employees
{
    /// <summary>
    /// DTO for results of stored procedure "EmployeeFilter"
    /// DTO для результатов хранимой процедуры "EmployeeFilter" 
    /// </summary>
    public class EmployeeViewDTO
    {
        /// <summary>
        /// Employee Id
        /// Id сотрудника
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Employee date of birth
        /// Дата рождения сотрудника
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Employee government identity code 
        /// Идентификационный код сотрудника
        /// </summary>

        public string IdentityCode { get; set; }
        /// <summary>
        /// First name from last issued passport
        /// Имя из последнего по дате выдачи паспорта
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Second name from last issued passport
        /// Отчество из последнего по дате выдачи паспорта
        /// </summary>

        public string SecondName { get; set; }
        /// <summary>
        /// Surname from last issued passport
        /// Фамилия из последнего по дате выдачи паспорта
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Short address of permanent residence in format of Country, City
        /// Краткий адрес проживания в формате Страна, Город
        /// </summary>

        public string ShortAddress { get; set; }
        /// <summary>
        /// Employee's age in int counted from date of birth
        /// Возраст сотрудника, вычисленный по дате рожения
        /// </summary>
        public int? AgeCount { get; set; }
        /// <summary>
        /// Username of AspNetUsers connected to Employee
        /// Имя логина сотрудника по связи в AspNetUsers
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Today positions of employee in format Position Department Office, separated by symbol '|'
        /// Текущие должности сотрудника в формате Должность Отдел Офис, отделенные друг от друга символом '|'
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// Employee's age in string for features of Russian language
        /// Возраст сотрудника в строковом формате, для указания "25 лет", "23 года" или "21 год" 
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// Collection of Positions of employee in format Position Department Office
        /// Коллекция строк, каждая из которых содержит в себе только одну должность в формате Должность Отдел Офис из всех должностей сотрудника
        /// </summary>
        public IEnumerable<String> PositionLines { get; set; }
          
    }
}