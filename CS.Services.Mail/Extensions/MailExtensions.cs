using CS.Services.Mail.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.Mail.Extensions
{
    public static class MailExtensions
    {
        public static IServiceCollection AddMailService(this IServiceCollection services, Action<MailSettings> configureSettings)
        {
            return services.AddSingleton<IMailService>(serviceProvider =>
            {
                var settings = new MailSettings();
                configureSettings(settings);
                return ActivatorUtilities.CreateInstance<MailService>(serviceProvider, settings);
            });
        }
    }
}
