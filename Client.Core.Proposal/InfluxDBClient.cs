using Client.Core.Proposal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfluxDB.Client
{
    public interface IInfluxDBClient
    {
        IWriteApi GetWriteApi();
    }
    public class InfluxDBClient : IInfluxDBClient
    {
        private InfluxDBClientOptions Options { get; }
        private IServiceProvider ServiceProvider { get; }

        public InfluxDBClient(InfluxDBClientOptions options)
        {
            Options = options;
            var services = new ServiceCollection();
            Registrations.ConfigureInfluxDB(services, options);
            ServiceProvider = services.BuildServiceProvider();
        }

        public IWriteApi GetWriteApi() => ServiceProvider.GetRequiredService<IWriteApi>();

    }
}
