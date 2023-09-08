using CS.Services.Mail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CS.Services.Mail.Interfaces;

namespace CS.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSettings settings;

        public MailService(MailSettings settings)
        {
            this.settings = settings;
        }
        public async Task<bool> SendAsync(MailData mail)
        {
            try
            {
                using MailMessage mailMessage = new MailMessage();
                using SmtpClient smtp = new SmtpClient(settings.SmtpServer);

                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Port = settings.SmtpPort;

                smtp.Credentials = new NetworkCredential(settings.SmtpUser, settings.SmtpPassword);

                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.None;

                mailMessage.From = new MailAddress(settings.SmtpUser);

                foreach (var to in mail.To)
                {
                    mailMessage.To.Add(to);
                }

                foreach (var coptyTo in mail.Cc)
                {
                    mailMessage.CC.Add(new MailAddress(coptyTo));
                }

                foreach (var hidenCopy in mail.Bcc)
                {
                    mail.Bcc.Add(hidenCopy);
                }

                mailMessage.Subject = mail.Subject;
                mailMessage.Body = mail.Body;
                mailMessage.IsBodyHtml = mail.IsHtml;

                await smtp.SendMailAsync(mailMessage).ConfigureAwait(false);

                return true; ;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
