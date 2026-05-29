namespace ActivityAssistent.App.Components.Pages.SalesConversations
{
    public class TeamActionPointDialogResult
    {
        public Guid ConversationId { get; set; }
        public ActionPointDialogModel ActionPoint { get; set; } = new();
    }
}
