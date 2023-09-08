using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.ServiceBusQueue
{
    public class ServiceBusSettings
    {
        public string QueueName { get; set; }
        public DateTime EnqueueTimeUtc { get; set; }
    }
}
