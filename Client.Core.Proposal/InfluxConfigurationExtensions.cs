using System;
using InfluxDB.Client;

// changed namespace to that of the IServiceCollection to allow for discoverability without using statements.
namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfluxConfigurationExtensions
    {
        public static IServiceCollection AddInfluxDb(this IServiceCollection services, Action<InfluxDBClientOptions.Builder> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            var builder = InfluxDBClientOptions.Builder.CreateNew();
            options(builder);
            var configured = builder.Build();

            var client = new InfluxDBClient(configured);
            services.AddSingleton<IInfluxDBClient>(client);
            
            return services;
        }
    }
}
