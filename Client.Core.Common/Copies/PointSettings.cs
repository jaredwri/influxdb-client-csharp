
using System;
using System.Collections.Generic;
using InfluxDB.Client.Core;

namespace InfluxDB.Common
{
    /// <summary>
    /// The setting for store data point: default values, threshold, ...
    /// </summary>
    public class PointSettings
    {
        private readonly SortedDictionary<string, string> _defaultTags =
            new SortedDictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        /// Add default tag. 
        /// </summary>
        /// <param name="key">the tag name</param>
        /// <param name="expression">the tag value expression</param>
        /// <returns>this</returns>
        public PointSettings AddDefaultTag(string key, string expression)
        {
            Arguments.CheckNotNull(key, "tagName");
            _defaultTags[key] = expression;
            return this;
        }

        // removed for example.
        public Dictionary<string, string> GetDefaultTags() => new Dictionary<string, string>();
    }
}