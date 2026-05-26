using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ActivityAssistent.Shared.Enums;

namespace ActivityAssistent.Shared.Dtos.Conversations
{
    public class UpdateConversationDto
    {
        [Required]

        public Guid ConversationId { get; set; }


        [Required(ErrorMessage = "Title is required.")]

        public string Title { get; set; }

        [Required(ErrorMessage = "Meeting date is required.")]

        public DateTime MeetingDate { get; set; }

        [Required(ErrorMessage = "Description is required."),MinLength(3) ,MaxLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public ConversationStatus Status { get; set; }
    }
}
