using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using InfluxDB.Client.Core.Internal;
using Newtonsoft.Json;

namespace InfluxDB.Client.Core.Exceptions
{
    public class InfluxException : Exception
    {
        public InfluxException(string message) : this(message, 0)
        {
        }

        public InfluxException(Exception exception) : base(exception.Message, exception)
        {
            Code = 0;
        }

        public InfluxException(string message, int code) : base(message)
        {
            Code = code;
            Status = 0;
        }

        /// <summary>
        ///     Gets the reference code unique to the error type. If the reference code is not present than return "0".
        /// </summary>
        public int Code { get; }

        /// <summary>
        ///     Gets the HTTP status code of the unsuccessful response. If the response is not present than return "0".
        /// </summary>
        public int Status { get; set; }
    }

    public class HttpException : InfluxException
    {
        public HttpException(string message, int status) : base(message, 0)
        {
            Status = status;
        }

        /// <summary>
        ///     The JSON unsuccessful response body.
        /// </summary>
        public JObject ErrorBody { get; set; }

        /// <summary>
        ///     The retry interval is used when the InfluxDB server does not specify "Retry-After" header.
        /// </summary>
        public int? RetryAfter { get; set; }

        //public static HttpException Create(HttpResponseMessage requestResult, object body)
        //{
        //    Arguments.CheckNotNull(requestResult, nameof(requestResult));

        //    var httpHeaders = LoggingHandler.ToHeaders(requestResult.Headers);

        //    return Create(body, httpHeaders, requestResult.ErrorMessage, requestResult.StatusCode);
        //}

        //public static HttpException Create(HttpResponseMessage requestResult, object body)
        //{
        //    Arguments.CheckNotNull(requestResult, nameof(requestResult));

        //    return Create(body, requestResult.Headers,  requestResult.ErrorMessage, requestResult.StatusCode);
        //}

        public static HttpException Create(HttpResponseMessage response)
        {
            Arguments.CheckNotNull(response, nameof(response));

            string stringBody = null;
            var errorBody = new JObject();
            string errorMessage = null;
            var headers = response.Headers;
            var content = response.Content;

            int? retryAfter = null;
            {
                var retryHeader = headers.RetryAfter;
                //var retryHeader = headers.FirstOrDefault(header => header.Name.Equals("Retry-After"));
                if (retryHeader != null) retryAfter = (int)retryHeader.Delta.GetValueOrDefault().TotalSeconds;
            }

            if (content != null)
            {
                stringBody = content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult() ?? "{}";
                //if (content is Stream)
                //{
                //    var stream = content as Stream;
                //    var sr = new StreamReader(stream);
                //    stringBody = sr.ReadToEnd();
                //}
                //else
                //{
                //    stringBody = content.ToString();
                //}
            }

            if (!string.IsNullOrEmpty(stringBody))
            {
                try
                {
                    errorBody = JObject.Parse(stringBody);
                    if (errorBody.ContainsKey("message"))
                    {
                        errorMessage = errorBody.GetValue("message").ToString();
                    }
                }
                catch (JsonException)
                {
                    errorBody = new JObject();
                }
            }

            var keys = new[] { "X-Platform-Error-Code", "X-Influx-Error", "X-InfluxDb-Error" };

            if (string.IsNullOrEmpty(errorMessage))
                errorMessage = headers
                    .Where(header => keys.Contains(header.Key, StringComparer.OrdinalIgnoreCase))
                    .Select(header => header.Value.ToString()).FirstOrDefault();

            //if (string.IsNullOrEmpty(errorMessage)) errorMessage = ErrorMessage;
            if (string.IsNullOrEmpty(errorMessage)) errorMessage = stringBody;

            return new HttpException(errorMessage, (int)response.StatusCode)
            { ErrorBody = errorBody, RetryAfter = retryAfter };
        }
    }
}
