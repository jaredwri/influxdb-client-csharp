using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;
using NodaTime;

namespace InfluxDB.Client.Internal
{
    internal class PropertyInfoColumn
    {
        internal PropertyInfo Property;
        internal Column Column;
    }


    internal class MeasurementMapper : IMeasurementMapper
    {
        private IDictionary<string, PropertyInfoColumn[]> CACHE = new ConcurrentDictionary<string, PropertyInfoColumn[]>();

        public PointData ToPoint<TM>(TM measurement, WritePrecision precision) where TM : class
        {
            Arguments.CheckNotNull(measurement, nameof(measurement));
            Arguments.CheckNotNull(precision, nameof(precision));

            var measurementType = measurement.GetType();
            CacheMeasurementClass(measurementType);

            var measurementAttribute = (Measurement)measurementType.GetCustomAttribute(typeof(Measurement));
            if (measurementAttribute == null)
            {
                throw new InvalidOperationException(
                    $"Measurement {measurement} does not have a {typeof(Measurement)} attribute.");
            }

            var point = PointData.Measurement(measurementAttribute.Name);

            foreach (var propertyInfo in CACHE[measurementType.Name])
            {
                var value = propertyInfo.Property.GetValue(measurement);
                if (value == null)
                {
                    continue;
                }

                var name = !string.IsNullOrEmpty(propertyInfo.Column.Name) ? propertyInfo.Column.Name : propertyInfo.Property.Name;
                if (propertyInfo.Column.IsTag)
                {
                    point.Tag(name, value.ToString());
                }
                else if (propertyInfo.Column.IsTimestamp)
                {
                    if (value is long l)
                    {
                        point.Timestamp(l, precision);
                    }
                    else if (value is TimeSpan span)
                    {
                        point.Timestamp(span, precision);
                    }
                    else if (value is DateTime date)
                    {
                        point.Timestamp(date, precision);
                    }
                    else if (value is DateTimeOffset offset)
                    {
                        point.Timestamp(offset, precision);
                    }
                    else if (value is Instant instant)
                    {
                        point.Timestamp(instant, precision);
                    }
                    else
                    {
                        Trace.WriteLine($"{value} is not supported as Timestamp");
                    }
                }
                else
                {
                    if (value is bool b)
                    {
                        point.Field(name, b);
                    }
                    else if (value is double d)
                    {
                        point.Field(name, d);
                    }
                    else if (value is float f)
                    {
                        point.Field(name, f);
                    }
                    else if (value is decimal dec)
                    {
                        point.Field(name, dec);
                    }
                    else if (value is long lng)
                    {
                        point.Field(name, lng);
                    }
                    else if (value is ulong ulng)
                    {
                        point.Field(name, ulng);
                    }
                    else if (value is int i)
                    {
                        point.Field(name, i);
                    }
                    else if (value is byte bt)
                    {
                        point.Field(name, bt);
                    }
                    else if (value is sbyte sb)
                    {
                        point.Field(name, sb);
                    }
                    else if (value is short sh)
                    {
                        point.Field(name, sh);
                    }
                    else if (value is uint ui)
                    {
                        point.Field(name, ui);
                    }
                    else if (value is ushort us)
                    {
                        point.Field(name, us);
                    }
                    else
                    {
                        point.Field(name, value.ToString());
                    }
                }
            }

            return point;
        }

        private void CacheMeasurementClass(Type measurementType)
        {
            if (CACHE.ContainsKey(measurementType.Name))
            {
                return;
            }

            CACHE[measurementType.Name] = measurementType.GetProperties()
                .Select(property => new PropertyInfoColumn { Column = (Column)property.GetCustomAttribute(typeof(Column)), Property = property })
                .Where(propertyInfo => propertyInfo.Column != null)
                .ToArray();
        }
    }
}