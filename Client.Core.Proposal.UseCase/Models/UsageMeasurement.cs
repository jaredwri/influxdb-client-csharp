using InfluxDB.Common;

namespace Client.Core.Proposal.UseCase.Models
{
    [Measurement("usage")]
    public class UsageMeasurement
    {
        [Column("serialNumber", IsTag = true)]
        public string SerialNumber { get; set; } = string.Empty;
        [Column("quantity")]
        public int Quantity { get; set; }
    }
}
