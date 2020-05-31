using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InfluxDB.Client.Api.Domain
{
    /// <summary>
    /// Defines WritePrecision
    /// </summary>

    [JsonConverter(typeof(StringEnumConverter))]

    public enum WritePrecision
    {
        /// <summary>
        /// Enum Ms for value: ms
        /// </summary>
        [EnumMember(Value = "ms")]
        Ms = 1,

        /// <summary>
        /// Enum S for value: s
        /// </summary>
        [EnumMember(Value = "s")]
        S = 2,

        /// <summary>
        /// Enum Us for value: us
        /// </summary>
        [EnumMember(Value = "us")]
        Us = 3,

        /// <summary>
        /// Enum Ns for value: ns
        /// </summary>
        [EnumMember(Value = "ns")]
        Ns = 4

    }

}
