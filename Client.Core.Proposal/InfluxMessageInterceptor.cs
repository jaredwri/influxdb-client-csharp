using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Core.Internal;

namespace Client.Core.Proposal
{
    internal class InfluxMessageInterceptor : DelegatingHandler
    {
        private ILogger<IWriteApi> Logger { get; }
        private LoggingHandler LoggingHandler { get; }

        public InfluxMessageInterceptor(
            ILogger<IWriteApi> logger,
            InfluxDBClientOptions options)
        {
            Logger = logger;
            LoggingHandler = new LoggingHandler(options.LogLevel);
            //  GzipHandler, removed from example.
            }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Before(request);
            var response = await base.SendAsync(request, cancellationToken);
            After(request, response);

            return response;
        }

        private async Task Before(HttpRequestMessage request)
        {
            await LoggingHandler.BeforeInterceptAsync(request);

            //_gzipHandler.BeforeIntercept(request);
            Logger.LogDebug("Intercept: Before");
        }

        private void After(HttpRequestMessage request, HttpResponseMessage response)
        {
            // not implemented; just demonstrating intercepting mechanics.
            //var uncompressed = _gzipHandler.AfterIntercept(statusCode, headers, body);
            //return (T)_loggingHandler.AfterIntercept(statusCode, headers, uncompressed);
            Logger.LogDebug("Intercept: After");
        }
    }
}
