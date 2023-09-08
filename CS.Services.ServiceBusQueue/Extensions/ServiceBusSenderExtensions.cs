using CS.Services.ServiceBusQueue.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.ServiceBusQueue.Extensions
{
    public static class ServiceBusSenderExtensions
    {
        public static IServiceCollection AddServiceBusSender(this IServiceCollection services, Action<ServiceBusSettings> configureSettings)
        {
            return services.AddSingleton<IServiceBusQueueSender>(serviceProvider =>
            {
                var settings = new ServiceBusSettings();
                configureSettings(settings);
                return ActivatorUtilities.CreateInstance<ServiceBusQueueSender>(serviceProvider, settings);
            });
        }
    }
}
