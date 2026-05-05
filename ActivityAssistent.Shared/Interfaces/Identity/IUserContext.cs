using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Interfaces.Identity
{
    public interface IUserContext
    {
        Guid CurrentUserId { get; }
    }
}
