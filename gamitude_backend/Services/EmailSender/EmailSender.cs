using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace gamitude_backend.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendVerificationEmailAsync(string email,string userName,string token);
        Task Execute(string apiKey, string subject, string messageHtml,string messagePlain, string email, string userName);

    }
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message,message, email,email);
        }
        public Task SendVerificationEmailAsync(string email,string userName,string token)
        {
            var subject = "Welcome to Gamitude! Confirm Your Email";
            
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            return Execute(Options.SendGridKey, subject,htmlContent, plainTextContent, email,userName);
        }

        public Task Execute(string apiKey, string subject, string messageHtml,string messagePlain, string email, string userName)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("gamitude@gamitude.rocks", "Gamitude"),
                Subject = subject,
                PlainTextContent = messagePlain,
                HtmlContent = messageHtml,

            };
            msg.AddTo(new EmailAddress(email,userName));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}