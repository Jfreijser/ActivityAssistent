using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Companies
{
    public class CompanyDto
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;


    }
}
