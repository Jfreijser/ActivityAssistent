using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.WebV2.Client.Interfaces.Identity
{
    public interface IUserContext
    {
        Guid CurrentUserId { get; }
    }
}
