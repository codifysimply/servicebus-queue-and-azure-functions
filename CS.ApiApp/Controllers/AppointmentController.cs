using CS.ApiApp.Models;
using CS.Services.ServiceBusQueue.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CS.ApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IServiceBusQueueSender serviceBusQueueSender;
        public AppointmentController(IServiceBusQueueSender serviceBusQueueSender)
        {
            this.serviceBusQueueSender = serviceBusQueueSender;
        }

        [HttpPost("book")]
        public async Task BookAppointment([FromBody] Appointment appointment)
        {
            await serviceBusQueueSender.SendMessageAsync(appointment).ConfigureAwait(false);
        }

        [HttpPost("book/batch")]
        public async Task BookAppointments([FromBody] List<Appointment> appointments)
        {
            await serviceBusQueueSender.SendMessageBatchAsync(appointments?.Cast<object>().ToList()).ConfigureAwait(false);
        }

        [HttpPost("schedule")]
        public async Task<long> ScheduledAppointment([FromBody] Appointment appointment, [FromQuery] DateTime enqueueTime)
        {
            var enqueueTimeutc = DateTime.UtcNow.AddMonths(2);
            return await serviceBusQueueSender.ScheduleMessageAsync(appointment, enqueueTimeutc).ConfigureAwait(false);
        }

        [HttpDelete("cancel/{sequenceNumber:long}")]
        public async Task CancelAppointmentNotificationFromQueue([FromRoute] int sequenceNumber)
        {
            await serviceBusQueueSender.CancelScheduledMessageAsync(sequenceNumber).ConfigureAwait(false);
        }
    }
}
