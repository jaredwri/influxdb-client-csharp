using InfluxDB.Client.Core;

namespace Client.Core.Proposal.UseCase.Models
{

    [Measurement("logout")]
    public class LogoutMeasurement
    {
        [Column("system", IsTag =true)]
        public string System { get; set; } = string.Empty;
        [Column("username")]
        public string Username { get; set; } = string.Empty;
    }
}
