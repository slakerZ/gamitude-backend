using gamitude_backend.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace gamitude_backend.Services
{
    public interface IEmailSender
    {
        // Task SendEmailAsync(string email, string subject, string message);
        Task<Response> SendVerificationEmailAsync(string email, string userName, string token,string templateId="d-ec00d4674dab420caa9b98276dec734a");
        Task<Response> Execute(string apiKey, SendGridMessage msg);


    }
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<EmailSenderSettings> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public EmailSenderSettings Options { get; } //set only via Secret Manager

        // public Task SendEmailAsync(string email, string subject, string message)
        // {
        //     return Execute(Options.SendGridKey, subject, message, message, email, email);
        // }
        public Task<Response> SendVerificationEmailAsync(string email, string userName, string link,string templateId)
        {
            link = Options.BaseUrl+link;
            var dynamicTemplateData = new templateData
            {
                name = userName,
                button_url = link
            };
            var to = new EmailAddress(email, userName);
            var from = new EmailAddress("noreply@gamitude.rocks", "Gamitude");
            var msg = MailHelper.CreateSingleTemplateEmail(from,to,templateId,dynamicTemplateData);
            return Execute(Options.SendGridKey, msg);
        }
        private class templateData
        {
            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("button_url")]
            public string button_url { get; set; }
        }
        public Task<Response> Execute(string apiKey, SendGridMessage msg)
        {
            var client = new SendGridClient(apiKey);
            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}