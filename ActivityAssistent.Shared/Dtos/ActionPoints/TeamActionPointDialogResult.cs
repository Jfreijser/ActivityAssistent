namespace ActivityAssistent.Shared.Dtos.ActionPoints
{
    public class TeamActionPointDialogResult
    {
        public Guid ConversationId { get; set; }
        public ActionPointDialogModel ActionPoint { get; set; } = new();
    }
}
