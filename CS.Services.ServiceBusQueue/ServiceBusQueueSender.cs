using Azure.Messaging.ServiceBus;
using CS.Services.ServiceBusQueue.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CS.Services.ServiceBusQueue
{
    public class ServiceBusQueueSender : IServiceBusQueueSender
    {
        private readonly ServiceBusSettings serviceBusSettings;
        private ServiceBusSender ServiceBusSender { get; set; }

        public ServiceBusQueueSender(ServiceBusClient serviceBusClient, ServiceBusSettings serviceBusSettings)
        {
            this.serviceBusSettings = serviceBusSettings;
            ServiceBusSender = serviceBusClient.CreateSender(serviceBusSettings.QueueName);
        }

        public async Task SendMessageAsync(object data = null)
        {
            var message = Create(data);
            await ServiceBusSender.SendMessageAsync(message).ConfigureAwait(false);
        }

        public async Task SendMessageBatchAsync(List<object> data = null)
        {
            var serviceBusMessageBatch = await ServiceBusSender.CreateMessageBatchAsync().ConfigureAwait(false);

            foreach (var item in data)
            {
                var message = Create(item);
                serviceBusMessageBatch.TryAddMessage(message);
            }

            await ServiceBusSender.SendMessagesAsync(serviceBusMessageBatch).ConfigureAwait(false);
        }

        public async Task<long> ScheduleMessageAsync(object data = null, DateTime? enqueueTimeUtc = null)
        {
            var message = Create(data);

            var enqueueTime = DateTime.UtcNow;

            if (enqueueTimeUtc != null)
            {
                enqueueTime = (DateTime)enqueueTimeUtc;
            }
            else
            {
                enqueueTime = (DateTime)serviceBusSettings.EnqueueTimeUtc;
            }

            return await ServiceBusSender.ScheduleMessageAsync(message, enqueueTime).ConfigureAwait(false);
        }

        public async Task CancelScheduledMessageAsync(long id)
        {
            await ServiceBusSender.CancelScheduledMessageAsync(id).ConfigureAwait(false);
        }

        private static ServiceBusMessage Create<T>(T data)
        {
            if (data == null)
            {
                return new ServiceBusMessage();
            }
            else
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented, settings);
                var content = new MemoryStream(Encoding.UTF8.GetBytes(json)).ToArray();
                var result = new ServiceBusMessage(content);

                return result;
            }
        }
    }
}
