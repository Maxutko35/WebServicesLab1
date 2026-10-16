using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;

namespace WebServicesLab1.Services
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
    }

    public class EmailService : IEmailSender
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                
                await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_settings.SenderEmail, _settings.SenderPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}