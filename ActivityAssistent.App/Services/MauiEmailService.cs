using System.Net;
using System.Net.Mail;
using ActivityAssistent.App.Interfaces.Email;
using Microsoft.Maui.Storage;

namespace ActivityAssistent.App.Services;

public sealed class MauiEmailService : IEmailService
{
    private const string SmtpHostKey = "smtp_host";
    private const string SmtpUserKey = "smtp_user";
    private const string SmtpPassKey = "smtp_pass";
    private const string SmtpSenderKey = "smtp_sender";
    private const string SmtpPortKey = "smtp_port";

    public async Task SendSummaryAsync(string recipientEmail, string subject, string body)
    {
        var settings = await LoadSettingsAsync();

        System.Diagnostics.Debug.WriteLine($"--- SMTP DEBUG ---");
        System.Diagnostics.Debug.WriteLine($"HOST: '{settings.Host}'");
        System.Diagnostics.Debug.WriteLine($"USER: '{settings.User}'");
        System.Diagnostics.Debug.WriteLine($"PASS LENGTE: {settings.Password.Length} tekens");
        System.Diagnostics.Debug.WriteLine($"------------------");

        if (string.IsNullOrWhiteSpace(settings.Host) || string.IsNullOrWhiteSpace(settings.SenderEmail))
        {
            throw new InvalidOperationException("SMTP settings are missing.");
        }

        try
        {
            using var message = new MailMessage(settings.SenderEmail, recipientEmail)
            {
                Subject = subject,
                Body = body
            };

            using var client = new SmtpClient(settings.Host, settings.Port);

            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(settings.User, settings.Password);
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

    private static async Task<(string Host, int Port, string User, string Password, string SenderEmail)> LoadSettingsAsync()
    {
        var host = Environment.GetEnvironmentVariable("AA_SMTP_HOST");
        var user = Environment.GetEnvironmentVariable("AA_SMTP_USER");
        var pass = Environment.GetEnvironmentVariable("AA_SMTP_PASS");
        var sender = Environment.GetEnvironmentVariable("AA_SMTP_SENDER");
        var portString = Environment.GetEnvironmentVariable("AA_SMTP_PORT");

        host ??= await SecureStorage.GetAsync(SmtpHostKey);
        user ??= await SecureStorage.GetAsync(SmtpUserKey);
        pass ??= await SecureStorage.GetAsync(SmtpPassKey);
        sender ??= await SecureStorage.GetAsync(SmtpSenderKey);
        portString ??= await SecureStorage.GetAsync(SmtpPortKey);

        var port = int.TryParse(portString, out var parsed) ? parsed : 587;

        return (host ?? string.Empty,
                port,
                user ?? string.Empty,
                pass ?? string.Empty,
                string.IsNullOrWhiteSpace(sender) ? (user ?? string.Empty) : sender);
    }
}