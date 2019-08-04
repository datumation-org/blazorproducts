using System.Threading.Tasks;
using datumation_products.Shared.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace datumation_products.Server.Services {
    public class AuthMessageSenderOptions {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }

    public class EmailSender : IEmailSender {
        //public EmailSender(IOptions optionsAccessor)
        //{
        //    Options = optionsAccessor.Value;
        //}
        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager
        public Task SendEmailAsync (string email, string subject, string htmlMessage) {
            return Execute (email, subject, htmlMessage);
        }
        public async Task<Response> Execute (string email, string subject, string htmlMessage) {
            var sendGridClient = new SendGridClient (ConfigurationFactory.Instance.Configuration ().AppSettings.AppConfiguration.Email.Smtp.ApiKey);
            var sendGridMessage = new SendGridMessage () {
                From = new EmailAddress ("noreply@datumation.com", "Datumation - Provider Data Sets"),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };
            sendGridMessage.AddTo (new EmailAddress (email));

            sendGridMessage.SetClickTracking (false, false);
            return await sendGridClient.SendEmailAsync (sendGridMessage);
        }

    }
}