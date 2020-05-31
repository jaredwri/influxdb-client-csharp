using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Common;

namespace Client.Core.Proposal.UseCase.Models
{
    [Measurement("login")]
    public class LoginMeasurement
    {
        [Column("system", IsTag =true)]
        public string System { get; set; } = string.Empty;
        [Column("username", IsTag = true)]
        public string Username { get; set; } = string.Empty;
        [Column("success")]
        public bool IsSuccess { get; set; }
    }
}
