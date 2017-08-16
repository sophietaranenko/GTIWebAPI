namespace GTIWebAPI.Models.Sales
{
    using Employees;
    using Organizations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InteractionActMember")]
    public partial class InteractionActMember
    {
        public int Id { get; set; }

        public int? InteractionActId { get; set; }

        public int? EmployeeId { get; set; }

        public virtual InteractionAct InteractionAct { get; set; }

        public virtual Employee Employee { get; set; }

        public InteractionActMemberDTO ToDTO()
        {
            return new InteractionActMemberDTO()
            {
                Id = this.Id,
                EmployeeId = this.EmployeeId,
                Employee = this.Employee == null ? null : this.Employee.ToShortForm(),
                InteractionActId = this.InteractionActId
            };
        }
    }

    public class InteractionActMemberDTO
    {
        public int Id { get; set; }

        public int? InteractionActId { get; set; }

        public int? EmployeeId { get; set; }

        public EmployeeShortForm Employee { get; set; }

        public InteractionActMember FromDTO()
        {
            return new InteractionActMember()
            {
                Id = this.Id,
                EmployeeId = this.EmployeeId,
                InteractionActId = this.InteractionActId
            };
        }

    }

    [Table("InteractionActOrganizationMember")]
    public partial class InteractionActOrganizationMember
    {
        public int Id { get; set; }

        public int? InteractionActId { get; set; }

        public int? OrganizationContactPersonId { get; set; }

        public virtual InteractionAct InteractionAct { get; set; }

        public virtual OrganizationContactPerson OrganizationContactPerson { get; set; }

        public InteractionActOrganizationMemberDTO ToDTO()
        {
            return new InteractionActOrganizationMemberDTO()
            {
                Id = this.Id,
                InteractionActId = this.InteractionActId,
                OrganizationContactPerson = this.OrganizationContactPerson == null ? null : this.OrganizationContactPerson.ToShortForm(),
                OrganizationContactPersonId = this.OrganizationContactPersonId
            };
        }
    }

    public class InteractionActOrganizationMemberDTO
    {
        public int Id { get; set; }

        public int? InteractionActId { get; set; }

        public int? OrganizationContactPersonId { get; set; }

        public OrganizationContactPersonShortForm OrganizationContactPerson { get; set; }

        public InteractionActOrganizationMember FromDTO()
        {
            return new InteractionActOrganizationMember()
            {
                Id = this.Id,
                OrganizationContactPersonId = this.OrganizationContactPersonId,
                InteractionActId = this.InteractionActId
            };
        }

    }
}
