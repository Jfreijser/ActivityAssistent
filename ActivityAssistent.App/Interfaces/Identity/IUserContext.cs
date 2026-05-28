using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.App.Interfaces.Identity
{
    public interface IUserContext
    {
        Guid CurrentUserId { get; }
    }
}
