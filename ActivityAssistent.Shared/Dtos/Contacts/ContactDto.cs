using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Contacts
{
    public class ContactDto
    {
        public Guid ContactId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
    }
}
