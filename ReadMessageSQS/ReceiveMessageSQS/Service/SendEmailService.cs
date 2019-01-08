using SendGrid;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using ReceiveMessageSQS.Models;
using ReceiveMessageSQS.Interfaces;

namespace ReceiveMessageSQS.Service
{
    public class SendEmailService : ISendEmailService
    {
        public Task SendEmail(string body)
        {
            var emailMessage = JsonConvert.DeserializeObject<EmailMessageModel>(body);

            var apiKey = "your_apiKey";
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("your_email_from", "your_name");
            var subject = emailMessage.Subject;
            var to = new EmailAddress(emailMessage.Email);
            var plainTextContent = "Teste para AWS";
            var htmlContent = $"<h2><strong>{emailMessage.Title}</strong></h2><p>{emailMessage.Message}</p><p>{body}</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = client.SendEmailAsync(msg);

            return response;
        }
    }
}
