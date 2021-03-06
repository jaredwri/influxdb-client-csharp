/* 
 * Influx API Service
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * OpenAPI spec version: 0.1.0
 * 
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenAPIDateConverter = InfluxDB.Client.Api.Client.OpenAPIDateConverter;

namespace InfluxDB.Client.Api.Domain
{
    /// <summary>
    /// Invites
    /// </summary>
    [DataContract]
    public partial class Invites :  IEquatable<Invites>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Invites" /> class.
        /// </summary>
        /// <param name="links">links.</param>
        /// <param name="invites">invites.</param>
        public Invites(InvitesLinks links = default(InvitesLinks), List<Invite> invites = default(List<Invite>))
        {
            this.Links = links;
            this._Invites = invites;
        }

        /// <summary>
        /// Gets or Sets Links
        /// </summary>
        [DataMember(Name="links", EmitDefaultValue=false)]
        public InvitesLinks Links { get; set; }

        /// <summary>
        /// Gets or Sets _Invites
        /// </summary>
        [DataMember(Name="invites", EmitDefaultValue=false)]
        public List<Invite> _Invites { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Invites {\n");
            sb.Append("  Links: ").Append(Links).Append("\n");
            sb.Append("  _Invites: ").Append(_Invites).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as Invites);
        }

        /// <summary>
        /// Returns true if Invites instances are equal
        /// </summary>
        /// <param name="input">Instance of Invites to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Invites input)
        {
            if (input == null)
                return false;

            return 
                (
                    
                    (this.Links != null &&
                    this.Links.Equals(input.Links))
                ) && 
                (
                    this._Invites == input._Invites ||
                    this._Invites != null &&
                    this._Invites.SequenceEqual(input._Invites)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Links != null)
                    hashCode = hashCode * 59 + this.Links.GetHashCode();
                if (this._Invites != null)
                    hashCode = hashCode * 59 + this._Invites.GetHashCode();
                return hashCode;
            }
        }

    }

}
