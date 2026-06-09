using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Agenda
{
    public class AgendaActionPointDto
    {
        public Guid ActionPointId { get; set; }
        public Guid ConversationId { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
