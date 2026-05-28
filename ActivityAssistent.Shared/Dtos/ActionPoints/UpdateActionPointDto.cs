using System;
using System.ComponentModel.DataAnnotations;

namespace ActivityAssistent.Shared.Dtos.ActionPoints
{
    public class UpdateActionPointDto
    {
        [Required(ErrorMessage = "Action point ID is required.")]
        public Guid ActionPointId { get; set; }

        [Required(ErrorMessage = "Conversation ID is required.")]
        public Guid ConversationId { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MinLength(3)]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Sales User ID is required.")]
        public Guid SalesUserId { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Completion status is required.")]
        public bool IsCompleted { get; set; }

        [Required(ErrorMessage = "SubNr ID is required.")]
        public Guid SubNrId { get; set; }
    }
}
