using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Api.Interfaces.Identity
{
    public interface IUserContext
    {
        Guid CurrentUserId { get; }
        string  Role { get; }
        Guid? SubNrId { get; }
    }
}
