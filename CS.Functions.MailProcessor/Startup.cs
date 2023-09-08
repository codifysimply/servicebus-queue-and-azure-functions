using CS.Services.Mail.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(CS.Functions.MailProcessor.Startup))]
namespace CS.Functions.MailProcessor
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            builder.Services.AddMailService(settings =>
            {
                settings.SmtpServer = configuration.GetValue<string>("SmtpServer");
                settings.SmtpPort = configuration.GetValue<int>("SmtpPort");
                settings.SmtpUser = configuration.GetValue<string>("SmtpUser"); ;
                settings.SmtpPassword = configuration.GetValue<string>("SmtpPassword"); ;
                settings.From = configuration.GetValue<string>("From");
            });
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            base.ConfigureAppConfiguration(builder);

            var context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(
                    Path.Combine(context.ApplicationRootPath, "appsettings.json"),
                    optional: true, 
                    reloadOnChange: false);
                
        }
    }
}
