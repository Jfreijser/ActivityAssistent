namespace ActivityAssistent.Shared.Dtos.ActionPoints
{
    public class ActionPointDialogModel
    {
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; } = DateTime.Today;
        public Guid SelectedSalesUserId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
