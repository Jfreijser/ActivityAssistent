using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Models
{
    public class ActionPoint
    {
        public Guid ActionPointId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }

        // De koppeling naar het gesprek
        public Guid SalesConversationId { get; set; }
    }
}
