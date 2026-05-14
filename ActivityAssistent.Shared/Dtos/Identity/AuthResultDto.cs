using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Identity
{
    public class AuthResultDto
    {
        public bool IsSuccess { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
    }
}
