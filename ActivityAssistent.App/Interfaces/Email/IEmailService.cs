namespace ActivityAssistent.App.Interfaces.Email;

public interface IEmailService
{
    Task SendSummaryAsync(string recipientEmail, string subject, string body);
}
