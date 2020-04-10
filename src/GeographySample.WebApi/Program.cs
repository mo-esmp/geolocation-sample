using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace GeographySample.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDb()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseNServiceBus(builderContext =>
                {
                    var endpointConfiguration = new EndpointConfiguration("GeographySample.Sender");
                    endpointConfiguration.EnableInstallers();
                    endpointConfiguration.EnableCallbacks();
                    endpointConfiguration.MakeInstanceUniquelyAddressable("GeographySample.Sender");
                    var scanner = endpointConfiguration.AssemblyScanner();
                    scanner.ScanAppDomainAssemblies = true;

                    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                    transport.UseConventionalRoutingTopology();
                    transport.ConnectionString(builderContext.Configuration["RabbitMqConnection"]);

                    return endpointConfiguration;
                });
    }
}