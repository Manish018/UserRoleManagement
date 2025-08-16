using System.Net;
using System.Net.Mail;

namespace UserRoleManagement.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;

        //IConfiguration is used to read values from application settings file for ex: appsettings.json
        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var from = _configuration["EmailSettings:From"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]!);
            var username = _configuration["EmailSettings:UserName"];
            var password = _configuration["EmailSettings:Password"];

            var message = new MailMessage(from!,toEmail,subject,body);
            //to make sure the message is in html format and not raw text
            message.IsBodyHtml = true;

            using var client = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(username, password),
                //to make sure credetials and email content are transmitted securly  as it enables SSL / TSL encryption btw app and SMTP server
                EnableSsl = true
            };
            await client.SendMailAsync(message);

        }
    }
}
