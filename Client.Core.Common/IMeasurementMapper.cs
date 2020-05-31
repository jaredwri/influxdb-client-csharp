namespace InfluxDB.Common
{
    public interface IMeasurementMapper
    {
        PointData ToPoint<T>(T measurement, WritePrecision precision) where T : class;
    }
}
