﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationPropertyDTO
    {
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? OrganizationPropertyTypeId { get; set; }

        public string Value { get; set; }

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public OrganizationPropertyTypeDTO OrganizationPropertyType { get; set; }
    }
}
