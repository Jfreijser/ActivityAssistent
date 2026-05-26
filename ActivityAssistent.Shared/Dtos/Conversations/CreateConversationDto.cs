using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ActivityAssistent.Shared.Enums;

namespace ActivityAssistent.Shared.Dtos.Conversations
{
    public class CreateConversationDto
    {
        [Required(ErrorMessage = "Title is required.")]

        public string Title { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        public Guid CompanyId { get; set; }
        [Required(ErrorMessage = "Sales User ID is required.")]
        public Guid SalesUserId { get; set; }


        public DateTime MeetingDate { get; set; } = DateTime.Now;
        public ConversationStatus Status { get; set; }
    }
}
