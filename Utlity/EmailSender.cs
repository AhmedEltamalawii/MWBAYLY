using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace MWBAYLY.Utlity
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("ahmedsherifeltamalawii@gmail.com", "znup eobc aswy vjtb")
            };

            return client.SendMailAsync(
                new MailMessage(from: "ahmedsherifeltamalawii@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
