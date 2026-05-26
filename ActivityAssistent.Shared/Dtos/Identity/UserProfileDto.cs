using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Identity
{
    public class UserProfileDto
    {
        public Guid? UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public Guid? SubNrId { get; set; }
    }
}
