using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HarperDBStudioMobile.Models
{
    public class InstanceSchemaModel
    {
        public Dictionary<string, Dictionary<string, InstanceSchema>> InstanceSchemaDict { get; set; }
    }

    public partial class InstanceSchema
    {
        [JsonProperty("__createdtime__", NullValueHandling = NullValueHandling.Ignore)]
        public long __createdtime__ { get; set; }

        [JsonProperty("__updatedtime__", NullValueHandling = NullValueHandling.Ignore)]
        public long __updatedtime__ { get; set; }

        [JsonProperty("hash_attribute", NullValueHandling = NullValueHandling.Ignore)]
        public string hash_attribute { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty("residence", NullValueHandling = NullValueHandling.Ignore)]
        public object residence { get; set; }

        [JsonProperty("schema", NullValueHandling = NullValueHandling.Ignore)]
        public string schema { get; set; }

        [JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        public List<SchemaAttribute> attributes { get; set; }

        [JsonProperty("record_count", NullValueHandling = NullValueHandling.Ignore)]
        public int record_count { get; set; }
    }

    public partial class SchemaAttribute
    {
        [JsonProperty("attribute", NullValueHandling = NullValueHandling.Ignore)]
        public string attribute { get; set; }
    }
}
