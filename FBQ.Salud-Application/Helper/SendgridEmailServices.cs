
using FBQ.Salud_Application.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace FBQ.Salud_Application.Helper
{
    public class SendgridEmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;

        public SendgridEmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Send(string toEmail, string fromEmail, string subject, string body)
        {
            var apiKey = _configuration.GetValue<string>("SENDGRID_API_KEY");

            var client = new SendGridClient(apiKey);

            var bodyFromLocal = System.IO.File.ReadAllText(@"..\OngProject\Templates\htmlpage.html");

            bodyFromLocal = bodyFromLocal.Replace("@correoContacto", fromEmail)
                                         .Replace("@bodyEmail", body)
                                         .Replace("@titulo", subject);

            var from = new EmailAddress(fromEmail);

            var to = new EmailAddress(toEmail);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, bodyFromLocal);

            var response = await client.SendEmailAsync(msg);
        }
    }
}
