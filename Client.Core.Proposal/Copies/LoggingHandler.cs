﻿using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace InfluxDB.Client.Core.Internal
{
    public class LoggingHandler
    {
        public LogLevel Level { get; set; }
        private ILogger Logger { get; }

        public LoggingHandler(LogLevel logLevel, ILogger logger)
        {
            Level = logLevel;
            Logger = logger;
        }

        private static IEnumerable<(string Name, string Value)> ToEnumerable(string query)
        {
            var values = HttpUtility.ParseQueryString(query);
            foreach (string key in values)
            {
                yield return (key, values[key]);
            }
        }

        private static IEnumerable<(string Name, string Value)> ToEnumerable(HttpRequestMessage request)
        {
            var result = request.Headers
                    .SelectMany(header =>
                        header.Value.Select(value => (header.Key, value))
            );

            return result;
        }

        public static IEnumerable<(string Name, string Value)> ToHeaders(HttpResponseHeaders headers)
        {
            var result = headers
                .SelectMany(header =>
                    header.Value.Select(value => (header.Key, value))
                );

            return result;
        }

        public async Task BeforeInterceptAsync(HttpRequestMessage request)
        {
            if (Level == LogLevel.None)
            {
                return;
            }

            var isBody = Level == LogLevel.Body;
            var isHeader = isBody || Level == LogLevel.Headers;

            Logger.LogDebug($"--> {request.Method} {request.RequestUri.AbsolutePath}");
            Trace.WriteLine($"--> {request.Method} {request.RequestUri.AbsolutePath}");

            if (isHeader)
            {
                LogHeaders(ToEnumerable(request.RequestUri.Query), "-->", "Query");

                var headers = ToEnumerable(request);
                LogHeaders(headers, "-->");
            }



            if (isBody)
            {

                var body = request.Content;

                //var body = request.Parameters.FirstOrDefault(parameter =>
                //    parameter.Type.Equals(ParameterType.RequestBody));

                if (body != null)
                {
                    var stringBody = await request.Content.ReadAsStringAsync();

                    //if (body.Value is byte[] bytes)
                    //{
                    //    stringBody = Encoding.UTF8.GetString(bytes);
                    //}
                    //else
                    //{
                    //    stringBody = body.Value.ToString();
                    //}

                    Trace.WriteLine($"--> Body: {stringBody}");
                    Logger.LogDebug($"--> Body: {stringBody}");
                }
            }

            Trace.WriteLine("--> END");
            Trace.WriteLine("-->");

            Logger.LogDebug("--> END");
            Logger.LogDebug("-->");
        }

        public object AfterIntercept(int statusCode, Func<HttpResponseHeaders> headers, object body)
        {
            var freshBody = body;
            if (Level == LogLevel.None)
            {
                return freshBody;
            }

            var isBody = Level == LogLevel.Body;
            var isHeader = isBody || Level == LogLevel.Headers;

            Trace.WriteLine($"<-- {statusCode}");
            Logger.LogDebug($"<-- {statusCode}");

            if (isHeader)
            {
                LogHeaders(ToHeaders(headers.Invoke()),"<--");
                //LogHeaders(headers.Invoke(), "<--");
            }

            if (isBody && body != null)
            {
                string stringBody;

                if (body is Stream)
                {
                    var stream = body as Stream;
                    var sr = new StreamReader(stream);
                    stringBody = sr.ReadToEnd();

                    freshBody = new MemoryStream(Encoding.UTF8.GetBytes(stringBody));
                }
                else
                {
                    stringBody = body.ToString();
                }

                if (!string.IsNullOrEmpty(stringBody))
                {
                    Trace.WriteLine($"<-- Body: {stringBody}");
                    Logger.LogDebug($"<-- Body: {stringBody}");
                }
            }


            Trace.WriteLine("<-- END");
            Logger.LogDebug("< --END");
            return freshBody;
        }

      

        private void LogHeaders(IEnumerable<(string Name, string Value)> values, string direction, string type = "Header")
        {
            foreach (var emp in values)
            {
                Trace.WriteLine($"{direction} {type}: {emp.Name}={emp.Value}");
                Logger.LogDebug($"{direction} {type}: {emp.Name}={emp.Value}");
            }
        }
    }
}