using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
namespace Vokimi.Services
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Vokimi", _smtpSettings.Username));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            if (isHtml)
                message.Body = new TextPart("html") { Text = body };
            else
                message.Body = new TextPart("plain") { Text = body };

            using (var client = ConfigureSmtpClient())
            {
                client.Send(message);
                client.Disconnect(true);
            }
        }
        public async Task SendConfirmationLink(string to, string confirmationLink)
        {
            string subject = "Please confirm your email";
            string body =
                "<p>Thank you for registering. Please click the link below to confirm your email:</p>" +
               $"<p><a href='{confirmationLink}'>Confirm Email</a></p>";

            await SendEmailAsync(to, subject, body, true);
        }
        private SmtpClient ConfigureSmtpClient()
        {
            var client = new SmtpClient();
            client.Connect(_smtpSettings.Host, _smtpSettings.Port, true);
            client.Authenticate(_smtpSettings.Username, _smtpSettings.Password);
            return client;
        }

    }
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

}