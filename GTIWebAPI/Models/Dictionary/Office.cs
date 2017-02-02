﻿using GTIWebAPI.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Dictionary
{
    [Table("Office")]
    public class Office 
    {
        public int Id { get; set; }

        public string NativeName { get; set; }

        public string ShortName { get; set; }

        public string EnglishName { get; set; }

        public string DealIndex { get; set; }

        public OfficeDTO ToDTO()
        {
            OfficeDTO dto = new OfficeDTO
            {
                Id = this.Id,
                ShortName = this.NativeName
            };
            return dto;
        }
    }
}
