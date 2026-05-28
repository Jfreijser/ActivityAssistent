using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.Contacts;
using ActivityAssistent.Shared.Dtos.Contacts;

namespace ActivityAssistent.App.Services
{
    public class MauiContactService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), IContactService
    {
        public Task<ContactDto> CreateContactAsync(ContactDto Contact, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<ContactDto> GetContactByIdAsync(Guid ContactId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ContactDto>> GetContactsByCompanyAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task UpdateContactAsync(ContactDto Contact, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
