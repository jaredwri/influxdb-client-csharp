using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.VisualBasic.CompilerServices;
using NodaTime.Text;

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

        private HttpClient CreateClient() => ClientFactory.CreateClient("InfluxDb-Write");

        private async Task SendAsync(HttpRequestMessage request)
        {
            var client = CreateClient();
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

       
        // other overloads/implementations here...
        public async Task WriteMeasurementAsync<T>(WritePrecision precision, T measurement) where T : class
        {
            // these will throw null reference if the builder doesn't assert configured options.
            await WriteMeasurementAsync(Options.Bucket, Options.Org, precision, measurement);
        }

        public async Task WriteMeasurementAsync<T>(string bucket, string org, WritePrecision precision, T measurement) where T : class
        {
            // assert stuff
            var request = RequestFactory.Create(bucket, org, precision, measurement);
            await SendAsync(request);
        }

        public async Task WritePointAsync(PointData pointData)
        {
            // these will throw null reference if the builder doesn't assert configured options.
            await WritePointAsync(Options.Bucket, Options.Org, pointData);
        }

        public async Task WritePointAsync(string bucket, string org, PointData pointData)
        {
            // assert stuff
            var request = RequestFactory.Create(bucket, org, pointData);
            await SendAsync(request);
        }

        public async Task WriteRecordAsync(WritePrecision precision, string record)
        {
            // these will throw null reference if the builder doesn't assert configured options.
            await WriteRecordAsync(Options.Bucket, Options.Org, WritePrecision.S, record);
        }

        public async Task WriteRecordAsync(string bucket, string org, WritePrecision precision, string record)
        {
            // assert stuff
            var request = RequestFactory.Create(bucket, org, precision, record);
            await SendAsync(request);
        }

        
    }
}
