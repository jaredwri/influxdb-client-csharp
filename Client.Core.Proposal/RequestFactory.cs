using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.AspNetCore.WebUtilities;

namespace Client.Core.Proposal
{

    public interface IRequestFactory
    {
        HttpRequestMessage Create(string lineProtocol, string org, string bucket, WritePrecision precision);
        HttpRequestMessage Create<T>(T measurement, string org, string bucket, WritePrecision precision) where T : class;
    }
    internal class RequestFactory : IRequestFactory
    {
        private IMeasurementMapper Mapper { get; }

        public RequestFactory(IMeasurementMapper mapper)
        {
            Mapper = mapper;
        }

        public HttpRequestMessage Create<T>(T measurement, string org, string bucket, WritePrecision precision) where T : class
        {
            var lineProtocol = Mapper
                .ToPoint(measurement, precision)
                .ToLineProtocol();

            return Create(lineProtocol, org, bucket, precision);
        }

        public HttpRequestMessage Create(string lineProtocol, string org, string bucket, WritePrecision precision)
        {
            var body = new LineProtocolContent(lineProtocol);

            var uri = QueryHelpers.AddQueryString("/api/v2/write", new Dictionary<string, string>
            {
                {InfluxApiParameters.Query.Org, WebUtility.UrlEncode(org)},
                {InfluxApiParameters.Query.Bucket, WebUtility.UrlEncode(bucket)},
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
