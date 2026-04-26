using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Conversations
{
    public class ConversationDto
    {
        public Guid ConversationId { get; set; }
        public string Title { get; set; }
        public string CustomerName { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Description { get; set; }

        // Alvast een status om aan te geven of de AI de samenvatting al klaar heeft
        public string Status { get; set; }
    }
}
