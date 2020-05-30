using Client.Core.Proposal;
using System;
using System.Runtime.CompilerServices;


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
            Registrations.Configure(services, configured);
            
            return services;
        }
    }
}
