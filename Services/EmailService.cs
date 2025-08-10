using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpUser = _config["Smtp:Username"];
        var smtpPass = _config["Smtp:Password"];
        var smtpHost = _config["Smtp:Host"];
        var smtpPort = int.Parse(_config["Smtp:Port"]);

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        };

        var fromName = _config["Smtp:FromName"];
        var fromEmail = _config["Smtp:FromEmail"];

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(to);

        await client.SendMailAsync(mailMessage);
    }
}
