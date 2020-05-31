using InfluxDB.Client.Writes;
using System.Threading.Tasks;
using InfluxDB.Client.Api.Domain;

namespace Client.Core.Proposal
{
    public interface IWriteApi
    {
        Task WriteMeasurementAsync<T>(WritePrecision precision, T measurement) where T : class;
        Task WriteMeasurementAsync<T>(string bucket, string org, WritePrecision precision, T measurement) where T : class;
        
        Task WritePointAsync(PointData pointData);
        Task WritePointAsync(string bucket, string org, PointData pointData);

        Task WriteRecordAsync(string bucket, string org, WritePrecision precision, string record);
        Task WriteRecordAsync(WritePrecision precision, string record);
    }
}
