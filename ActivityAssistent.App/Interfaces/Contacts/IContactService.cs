using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Dtos.Contacts;

namespace ActivityAssistent.App.Interfaces.Contacts
{
    public interface IContactService
    {
        Task<ContactDto> GetContactByIdAsync(Guid ContactId, CancellationToken Token);
        Task<IEnumerable<ContactDto>> GetContactsByCompanyAsync(Guid CompanyId, CancellationToken Token);
        Task<ContactDto> CreateContactAsync(ContactDto Contact, CancellationToken Token);
        Task UpdateContactAsync(ContactDto Contact, CancellationToken Token);
    }
}
