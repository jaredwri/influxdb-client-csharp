using System.Net.Http;
using System.Threading.Tasks;
using InfluxDB.Client.Api.Domain;

namespace Client.Core.Proposal
{
    internal class WriteApi : IWriteApi
    {
        private IHttpClientFactory ClientFactory { get; }
        private IRequestFactory RequestFactory { get; }
        private InfluxDBClientOptions Options { get; }

        public WriteApi(
            IHttpClientFactory clientFactory,
            IRequestFactory requestFactory,
            InfluxDBClientOptions options)
        {
            ClientFactory = clientFactory;
            RequestFactory = requestFactory;
            Options = options;
        }
        public async Task WriteAsync<T>(T measurement) where T : class
        {
            var request = RequestFactory.Create(measurement, Options.Org,Options.Bucket, WritePrecision.S);
            var client = ClientFactory.CreateClient("InfluxDb-Write");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        // other overloads/implementations here...
    }
}
