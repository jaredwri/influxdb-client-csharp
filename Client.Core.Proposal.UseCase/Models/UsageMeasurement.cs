using InfluxDB.Client.Core;

namespace Client.Core.Proposal.UseCase.Models
{
    [Measurement("usage")]
    public class UsageMeasurement
    {
        [Column("serialNumber", IsTag = true)]
        public string SerialNumber { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
    }
}
