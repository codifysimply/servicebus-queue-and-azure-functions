using System;
using CS.Services.Mail.Models;
using CS.Services.Mail;
using Microsoft.Azure.Amqp.Framing;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CS.Functions.MailProcessor.Models;
using System.Threading.Tasks;
using CS.Services.Mail.Interfaces;
using Microsoft.Azure.WebJobs;

namespace CS.Functions.MailProcessor
{
    public class ProcessMail
    {
        private readonly IMailService mailService;

        public ProcessMail(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [FunctionName("ProcessMail")]
        public async Task RunAsync([ServiceBusTrigger("csblog-email-queue", Connection = "ServiceBusQueueConnectionString")]string myQueueItem, ILogger log)
        {
            if (myQueueItem != null)
            {
                var mail = Create(myQueueItem);
                await mailService.SendAsync(mail).ConfigureAwait(false);
            }
        }

        private static MailData Create(string data)
        {
            var appointment = JsonConvert.DeserializeObject<Appointment>(data);
            var mail = new MailData();
            mail.To = new List<string> { appointment.PatientEmail };
            mail.Subject = $"Appointment reminder for {appointment.AppointmentStart:g}";
            mail.IsHtml = true;

            StringBuilder sb = new StringBuilder();
            sb.Append($"Dear Mr./Mrs. {appointment.PatientFirstName} {appointment.PatientLastName}<br>");
            sb.Append($"This is a reminder that you have an appointment scheduled for ");
            sb.Append($"{appointment.AppointmentStart.ToString("dd.MM.yyy")} at {appointment.AppointmentStart.ToString("H:mm")}<br>");
            sb.Append("We look forward to seeing you.<br>");
            sb.Append("Best regards.");

            mail.Body = sb.ToString();
            return mail; 
        }
    }
}
