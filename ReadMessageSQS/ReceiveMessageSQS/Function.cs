using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Amazon.Lambda.SQSEvents;
using ReceiveMessageSQS.Service;
using ReceiveMessageSQS.Interfaces;
using Microsoft.Extensions.DependencyInjection;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ReceiveMessageSQS
{
    public class Function
    {
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            context.Logger.LogLine($"Configure Services - Dependency Injection");
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            context.Logger.LogLine($"Build Service Provider - Dependency Injection");
            var serviceProvider = serviceCollection.BuildServiceProvider();

            foreach (var message in evnt.Records)
            {
                context.Logger.LogLine($"Processed message {message.Body}");
                await serviceProvider.GetService<App>().Run(message.Body);
                context.Logger.LogLine($"Processed message end");
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ISendEmailService, SendEmailService>();
            serviceCollection.AddTransient<App>();
        }
    }

    public class App
    {
        private readonly ISendEmailService _sendEmailServiceAcl;

        public App(ISendEmailService sendEmailServiceAcl)
        {
            _sendEmailServiceAcl = sendEmailServiceAcl;
        }

        public Task Run(string body)
        {
            return _sendEmailServiceAcl.SendEmail(body);
        }
    }
}
