using System.Collections.Generic;
using System.Net.Http;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.AspNetCore.WebUtilities;

namespace Client.Core.Proposal
{

    public interface IRequestFactory
    {
        HttpRequestMessage Create(string bucket, string org, WritePrecision precision, string lineProtocol);
        HttpRequestMessage Create(string bucket, string org, PointData data);
        HttpRequestMessage Create<T>(string bucket, string org, WritePrecision precision, T measurement) where T : class;
    }
    internal class RequestFactory : IRequestFactory
    {
        private IMeasurementMapper Mapper { get; }

        public RequestFactory(IMeasurementMapper mapper)
        {
            Mapper = mapper;
        }

        public HttpRequestMessage Create<T>(string bucket, string org, WritePrecision precision, T measurement) where T : class
        {
            var point = Mapper.ToPoint(measurement, precision);
            return Create(bucket, org, point);
        }

        public HttpRequestMessage Create(string bucket, string org, PointData data)
        {
            var lineProtocol = data.ToLineProtocol();
            return Create(bucket, org, data.Precision, lineProtocol);
        }

        public HttpRequestMessage Create(string bucket, string org, WritePrecision precision, string lineProtocol)
        {
            var body = new LineProtocolContent(lineProtocol);

            // pretty sure this performs url encode on each parameter.
            var uri = QueryHelpers.AddQueryString("/api/v2/write", new Dictionary<string, string>
            {
                {InfluxApiParameters.Query.Org, org},
                {InfluxApiParameters.Query.Bucket, bucket},
                {InfluxApiParameters.Query.Precision, precision.ToString().ToLower()},
            });

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = body,
            };

            return request;
        }
    }
}
