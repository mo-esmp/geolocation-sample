using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace GeographySample.DistanceCalculator
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.StartAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .UseNServiceBus(builderContext =>
                    {
                        var endpointConfiguration = new EndpointConfiguration("GeographySample.Receiver");
                        endpointConfiguration.EnableInstallers();

                        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                        transport.UseConventionalRoutingTopology();

                        Console.WriteLine("RabbitMqConnection is:" + builderContext.Configuration["RabbitMqConnection"]);
                        transport.ConnectionString(builderContext.Configuration["RabbitMqConnection"]);

                        return endpointConfiguration;
                    });
    }
}