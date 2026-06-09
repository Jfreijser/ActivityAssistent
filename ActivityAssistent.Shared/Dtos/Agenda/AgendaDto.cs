using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Agenda
{
    public class AgendaDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public List<AgendaActionPointDto> ActionPoints { get; set; } = new List<AgendaActionPointDto>();
    }

}
