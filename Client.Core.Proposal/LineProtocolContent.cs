using System;
using System.Net.Http;
using System.Text;

namespace InfluxDB.Client
{
    internal class LineProtocolContent : ByteArrayContent
    {
        public LineProtocolContent(string lineProtocolBody)
            : base(Encoding.UTF8.GetBytes(lineProtocolBody))
        {
            if (string.IsNullOrWhiteSpace(lineProtocolBody)) throw new ArgumentOutOfRangeException(nameof(lineProtocolBody), "Body cannot be null");
            Headers.Add("Content-Encoding", "identity");
            Headers.Add("Content-Type", "text/plain; charset=utf-8");
        }

    }
}
