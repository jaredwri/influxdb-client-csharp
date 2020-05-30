using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Client.Core.Internal;

namespace Client.Core.Proposal
{
    internal class InfluxMessageInterceptor : DelegatingHandler
    {
        private ILogger<IWriteApi> Logger { get; }
        private LoggingHandler LoggingHandler { get; }
        //private GzipHandler? GzipHandler { get; }

        public InfluxMessageInterceptor(
            ILogger<IWriteApi> logger,
            InfluxDBClientOptions options)
        {
            Logger = logger;
            // the things 
            LoggingHandler = new LoggingHandler(options.LogLevel, logger);
            //GzipHandler = new GzipHandler();
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Before(request, cancellationToken);
            var response = await base.SendAsync(request, cancellationToken);
            After(request, response, cancellationToken);

            return response;
        }

        private async Task Before(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await LoggingHandler.BeforeInterceptAsync(request);
            //_gzipHandler.BeforeIntercept(request);
            Logger.LogDebug("Intercept: Before");
        }

        private void After(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            // not implemented; just demonstrating intercepting mechanics.
            //var uncompressed = _gzipHandler.AfterIntercept(statusCode, headers, body);
            //return (T)_loggingHandler.AfterIntercept(statusCode, headers, uncompressed);
            Logger.LogDebug("Intercept: After");
        }
    }
}
