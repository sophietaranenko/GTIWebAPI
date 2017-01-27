﻿using GTIWebAPI.Models.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationAddressDTO
    {
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? OrganizationAddressTypeId { get; set; }

        public int? AddressId { get; set; }

        public AddressDTO Address { get; set; } 

        public OrganizationAddressTypeDTO OrganizationAddressType { get; set; }
    }
}