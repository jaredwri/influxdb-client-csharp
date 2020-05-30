using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Core.Proposal.UseCase
{
    public class InfluxSettings
    {
        public string Url { get; set; }
        public string Token { get; set; }
        public string Bucket { get; set; }
        public string Org { get; set; }
        public int JitterInterval { get; set; }

    }
}
