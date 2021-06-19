using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HarperDBStudioMobile.Models
{
    public class OrgModel
    {
        [JsonProperty("customer_id")]
        public string customer_id { get; set; }

        [JsonProperty("customer_name")]
        public string customer_name { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("owner_user_id")]
        public Guid owner_user_id { get; set; }

        [JsonProperty("total_instance_count")]
        public long total_instance_count { get; set; }

        [JsonProperty("free_cloud_instance_count")]
        public long free_cloud_instance_count { get; set; }

        [JsonProperty("active_coupons")]
        public List<object> active_coupons { get; set; }
    }
}
