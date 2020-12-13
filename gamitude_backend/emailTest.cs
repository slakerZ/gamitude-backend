// using SendGrid's C# Library
// https://github.com/sendgrid/sendgrid-csharp
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace gamitude_backend
{
    class Example
    {

        public async Task Execute()
        {
            var apiKey = "";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("gamitude@gamitude.rocks", "Gamitude");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("stas.lutkiewicz@gmail.com", "Stanisław Lutkiewicz");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            Console.WriteLine(response.Body+response.StatusCode.ToString());
        }
    }
}
