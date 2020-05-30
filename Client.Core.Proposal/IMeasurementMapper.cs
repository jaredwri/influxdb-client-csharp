using System;
using System.Collections.Generic;
using System.Text;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace InfluxDB.Client
{
    public interface IMeasurementMapper
    {
        PointData ToPoint<T>(T measurement, WritePrecision precision) where T : class;
    }
}
