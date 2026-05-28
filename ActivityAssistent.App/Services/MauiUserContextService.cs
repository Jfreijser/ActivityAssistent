using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.Identity;

namespace ActivityAssistent.App.Services
{
    public class MauiUserContextService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), IUserContext
    {
        public Guid CurrentUserId => throw new NotImplementedException();
    }
}
