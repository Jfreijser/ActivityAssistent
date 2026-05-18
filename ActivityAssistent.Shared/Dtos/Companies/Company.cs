using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Companies
{
    public class Company
    {
        
        public Guid? CompanyId { get; set; }
        public string? Name { get; set; }
        public string? KvkNumber { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? City { get; set; } 
        public string? Address { get; set; }
        public Guid? CompanyContact { get; set; }


    }
}
