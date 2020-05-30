using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using InfluxDB.Client;
using InfluxDB.Client.Internal;
using InfluxDB.Client.Core.Internal;

namespace Client.Core.Proposal
{
    internal static class Registrations
    {
        public static void Configure(IServiceCollection services, InfluxDBClientOptions options)
        {
            services.TryAddSingleton(options);
            services.TryAddSingleton<IMeasurementMapper, MeasurementMapper>();
            services.TryAddTransient<IRequestFactory, RequestFactory>();
            services.TryAddSingleton<IWriteApi, WriteApi>();
            services.TryAddTransient<InfluxMessageInterceptor>();

            services.AddHttpClient("InfluxDb-Write", configuration =>
                {
                    configuration.BaseAddress = new Uri(options.Url);
                    configuration.DefaultRequestHeaders.Add("Authorization", $"Token {options.Token}");
                    configuration.DefaultRequestHeaders.Add("Accept", "application/json");
                    var version = "proposal";
                    configuration.DefaultRequestHeaders.Add("User-Agent", $"influxdb-client-csharp/{version}");
                    // user agent, read-write timeout, timeout, etc.
                })
                .AddHttpMessageHandler<InfluxMessageInterceptor>()
                .AddPolicyHandler(Policies.BusyPolicy)
                .AddPolicyHandler(Policies.BadRequest)
                .AddPolicyHandler(Policies.RequestEntityTooLarge)
                .AddPolicyHandler(Policies.Default);
        }
    }
}
