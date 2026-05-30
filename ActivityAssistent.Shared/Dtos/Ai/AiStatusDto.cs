using System;
using System.Collections.Generic;
using System.Text;
using ActivityAssistent.Shared.Enums;

namespace ActivityAssistent.Shared.Dtos.Ai
{
    public class AiStatusDto
    {
        public Guid Token { get; set; }
        public AiStatus CurrentState { get; set; }

        public string StatusMessage { get; set; }

    }
}
