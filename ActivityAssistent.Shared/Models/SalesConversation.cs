using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Models
{
    public class SalesConversation
    {
        public Guid SalesConversationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime MeetingDate { get; set; } = DateTime.Now;
        public string SalesPersonId { get; set; } = string.Empty;
        public string? RecordingBlobUri { get; set; }
        public string? AiSummary { get; set; }
    }
}
