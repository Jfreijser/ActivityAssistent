using System.Net;
using System.Net.Mail;
using ActivityAssistent.App.Interfaces.Email;

namespace ActivityAssistent.App.Services;

public sealed class MauiEmailService : IEmailService
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPassword;
    private readonly string _senderEmail;

    public MauiEmailService()
    {
        _smtpHost = Environment.GetEnvironmentVariable("AA_SMTP_HOST") ?? string.Empty;
        _smtpUser = Environment.GetEnvironmentVariable("AA_SMTP_USER") ?? string.Empty;
        _smtpPassword = Environment.GetEnvironmentVariable("AA_SMTP_PASS") ?? string.Empty;
        _senderEmail = Environment.GetEnvironmentVariable("AA_SMTP_SENDER") ?? _smtpUser;
        _smtpPort = int.TryParse(Environment.GetEnvironmentVariable("AA_SMTP_PORT"), out var port) ? port : 587;
    }

    public async Task SendSummaryAsync(string recipientEmail, string subject, string body)
    {
        System.Diagnostics.Debug.WriteLine($"--- SMTP DEBUG ---");
        System.Diagnostics.Debug.WriteLine($"HOST: '{_smtpHost}'");
        System.Diagnostics.Debug.WriteLine($"USER: '{_smtpUser}'");
        System.Diagnostics.Debug.WriteLine($"PASS LENGTE: {_smtpPassword.Length} tekens"); // We printen alleen de lengte, wel zo veilig
        System.Diagnostics.Debug.WriteLine($"------------------");

        if (string.IsNullOrWhiteSpace(_smtpHost) || string.IsNullOrWhiteSpace(_senderEmail))
        {
            throw new InvalidOperationException("SMTP settings are missing.");
        }

        try
        {
            using var message = new MailMessage(_senderEmail, recipientEmail)
            {
                Subject = subject,
                Body = body
            };

            using var client = new SmtpClient(_smtpHost, _smtpPort);

            client.UseDefaultCredentials = false;
            // Pas daarna geven we het wachtwoord
            client.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            await client.SendMailAsync(message);
            System.Diagnostics.Debug.WriteLine("SUCCES: De e-mail is succesvol verzonden!");
        }
        catch (SmtpException ex)
        {
            System.Diagnostics.Debug.WriteLine($"SMTP FOUT: {ex.StatusCode} - {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"EXTRA DETAILS: {ex.InnerException?.Message}");
        }
    }
}
