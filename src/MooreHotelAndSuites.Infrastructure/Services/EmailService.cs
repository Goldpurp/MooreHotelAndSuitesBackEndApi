using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using MooreHotelAndSuites.Application.Interfaces.Services;

namespace MooreHotelAndSuites.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var smtp = new SmtpClient
            {
                Host = _config["Smtp:Host"]!,
                Port = int.Parse(_config["Smtp:Port"]!),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _config["Smtp:Username"],
                    _config["Smtp:Password"]
                )
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_config["Smtp:From"]!, "Moore Hotel"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mail.To.Add(to);

            await smtp.SendMailAsync(mail);
        }
    }
}
