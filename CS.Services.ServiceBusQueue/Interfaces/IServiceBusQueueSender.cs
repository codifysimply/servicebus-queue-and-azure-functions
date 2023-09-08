using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.ServiceBusQueue.Interfaces
{
    public interface IServiceBusQueueSender
    {
        Task SendMessageAsync(object data = null);
        Task SendMessageBatchAsync(List<object> data = null);
        Task<long> ScheduleMessageAsync(object data = null, DateTime? enqueueTimeUtc = null);
        Task CancelScheduledMessageAsync(long id);
    }
}
