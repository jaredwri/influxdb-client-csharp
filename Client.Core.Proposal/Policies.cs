using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InfluxDB.Client.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Client.Core.Proposal
{
    internal static class Policies
    {

        internal static IAsyncPolicy<HttpResponseMessage> BusyPolicy(IServiceProvider serviceProvider, HttpRequestMessage message)
        {
            // message is not used, simply using the .AddPolicyHandler overload that best matched to allow for method group syntax in configuration
            var result = Policy<HttpResponseMessage>
                .HandleResult(response =>
                    response.StatusCode == System.Net.HttpStatusCode.TooManyRequests ||
                    response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                .RetryForeverAsync((response, number, context) =>
                {
                    var log = serviceProvider.GetService<ILogger<IWriteApi>>();
                    var sleepDuration = response.Result.Headers?.RetryAfter?.Delta ?? GetJitterInterval(serviceProvider);
                    log?.LogInformation(
                        "Retry http client operation in {seconds} seconds due to http status code of {statusCode}",
                        sleepDuration.TotalSeconds, response.Result.StatusCode);
                    return Task.Delay(sleepDuration);
                });

            return result;
        }

        private static TimeSpan GetJitterInterval(IServiceProvider serviceProvider)
        {
            var settings = serviceProvider.GetRequiredService<InfluxDBClientOptions>();


            if (settings.JitterInterval <= 0) return TimeSpan.Zero;

            var interval = (int)(new Random().NextDouble() * settings.JitterInterval);
            var jitterDelay = TimeSpan.FromMilliseconds(interval);
            return jitterDelay;
        }

        internal static IAsyncPolicy<HttpResponseMessage> BadRequest(IServiceProvider serviceProvider,
            HttpRequestMessage message)
        {
            var result = Policy<HttpResponseMessage>
                .HandleResult(response => response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                .FallbackAsync(
                    fallbackValue: new HttpResponseMessage(),
                    onFallbackAsync: (response, context) => throw HttpException.Create(response.Result));
            return result;
        }

        internal static IAsyncPolicy<HttpResponseMessage> RequestEntityTooLarge(IServiceProvider serviceProvider,
            HttpRequestMessage message)
        {
            var result = Policy<HttpResponseMessage>
                .HandleResult(response => response.StatusCode == System.Net.HttpStatusCode.RequestEntityTooLarge)
                .FallbackAsync(
                    fallbackValue: new HttpResponseMessage(),
                    onFallbackAsync: (response, context) => throw HttpException.Create(response.Result)
                );
            return result;
        }


        internal static IAsyncPolicy<HttpResponseMessage> Default(IServiceProvider serviceProvider,
            HttpRequestMessage message)
        {
            // this handles the default case as well as 401 and 403.   really anything with a code/message payload.
            var result = Policy<HttpResponseMessage>
                .HandleResult(response => !response.IsSuccessStatusCode)
                .FallbackAsync(
                    fallbackValue: new HttpResponseMessage(),
                    onFallbackAsync: (response, context) => throw HttpException.Create(response.Result)
                );
            return result;
        }

    }
}
