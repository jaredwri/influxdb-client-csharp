using System;
using InfluxDB.Common;

namespace InfluxDB.Client
{
    public static class InfluxDBClientFactory
    {
        /// <summary>
        /// Create a instance of the InfluxDB 2.0 client.
        /// </summary>
        /// <param name="url">the url to connect to the InfluxDB 2.0</param>
        /// <param name="token">the token to use for the authorization</param>
        /// <returns>client</returns>
        public static IInfluxDBClient Create(string url, string token)
        {
            var options = InfluxDBClientOptions.Builder
                .CreateNew()
                .Url(url)
                .AuthenticateToken(token)
                .Build();

            return Create(options);
        }

        /// <summary>
        /// Create a instance of the InfluxDB 2.0 client.
        /// </summary>
        /// <param name="options">the connection configuration</param>
        /// <returns>client</returns>
        public static IInfluxDBClient Create(InfluxDBClientOptions options)
        {
            Arguments.CheckNotNull(options, nameof(options));

            return new InfluxDBClient(options);
        }
        public static IInfluxDBClient Create(Action<InfluxDBClientOptions.Builder> configuration)
        {
            var builder = InfluxDBClientOptions.Builder.CreateNew();
            configuration?.Invoke(builder);
            return new InfluxDBClient(builder.Build());
        }

        public static IInfluxDBClient Create(string url, string org, string bucket, string token)
            => Create(builder =>
            {
                builder.Url(url).Org(org).Bucket(bucket).AuthenticateToken(token);
            });
    }
}