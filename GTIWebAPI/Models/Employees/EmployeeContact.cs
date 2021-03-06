﻿namespace GTIWebAPI.Models.Employees
{
    using Service;
    using Personnel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeContact")]
    public partial class EmployeeContact : GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public int? ContactTypeId { get; set; }

        public virtual ContactType ContactType { get; set; }

        [StringLength(250)]
        public string Value { get; set; }

        public bool? Deleted { get; set; }

        protected override string TableName
        {
            get
            {
                return "EmployeeContact";
            }
        }

        public EmployeeContactDTO ToDTO()
        {
            EmployeeContactDTO dto = new EmployeeContactDTO()
            {
                ContactType = this.ContactType == null ? null : this.ContactType.ToDTO(),
                ContactTypeId = this.ContactTypeId,
                EmployeeId = this.EmployeeId,
                Id = this.Id,
                Value = this.Value
            };
            return dto;
        }
    }

    public class EmployeeContactDTO
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public int? ContactTypeId { get; set; }

        public ContactTypeDTO ContactType { get; set; }

        public string Value { get; set; }

        public EmployeeContact FromDTO()
        {
            return new EmployeeContact()
            {
                ContactTypeId = this.ContactTypeId,
                EmployeeId = this.EmployeeId,
                Id = this.Id,
                Value = this.Value
            };
        }

    }
}
