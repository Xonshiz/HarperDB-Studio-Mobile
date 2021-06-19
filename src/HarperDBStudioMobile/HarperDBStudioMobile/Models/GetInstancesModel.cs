using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Instances
    ///
    /// POST https://prod.harperdbcloudservices.com/getInstances
    /// </summary>
    public class GetInstancesModel
{
        [JsonProperty("body")]
        public List<InstanceModel> body { get; set; }
    }

    public class InstanceModel
    {
        [JsonProperty("rule_arn")]
        public string rule_arn { get; set; }

        [JsonProperty("port")]
        public object port { get; set; }

        [JsonProperty("instance_tier")]
        public string instance_tier { get; set; }

        [JsonProperty("stripe_storage_plan_id")]
        public string stripe_storage_plan_id { get; set; }

        [JsonProperty("storage_subscription_id")]
        public object storage_subscription_id { get; set; }

        [JsonProperty("compute_stack_id")]
        public string compute_stack_id { get; set; }

        [JsonProperty("clustering_target_group_arn")]
        public string clustering_target_group_arn { get; set; }

        [JsonProperty("stripe_plan_id")]
        public string stripe_plan_id { get; set; }

        [JsonProperty("clustering_rule_arn")]
        public string clustering_rule_arn { get; set; }

        [JsonProperty("creation_date")]
        public DateTime creation_date { get; set; }

        [JsonProperty("is_ssl")]
        public bool is_ssl { get; set; }

        [JsonProperty("storage_sub_item_id")]
        public string storage_sub_item_id { get; set; }

        [JsonProperty("network_stack_id")]
        public string network_stack_id { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("compute_sub_item_id")]
        public string compute_sub_item_id { get; set; }

        [JsonProperty("private_ip")]
        public string private_ip { get; set; }

        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("host")]
        public object host { get; set; }

        [JsonProperty("instance_name")]
        public string instance_name { get; set; }

        [JsonProperty("compute_subscription_id")]
        public object compute_subscription_id { get; set; }

        [JsonProperty("instance_id")]
        public string instance_id { get; set; }

        [JsonProperty("last_volume_resize")]
        public object last_volume_resize { get; set; }

        [JsonProperty("stripe_subscription_id")]
        public string stripe_subscription_id { get; set; }

        [JsonProperty("target_group_arn")]
        public string target_group_arn { get; set; }

        [JsonProperty("instance_region")]
        public string instance_region { get; set; }

        [JsonProperty("compute_connect_stack_id")]
        public string compute_connect_stack_id { get; set; }

        [JsonProperty("data_volume_size")]
        public long data_volume_size { get; set; }

        [JsonProperty("hidden")]
        public object hidden { get; set; }

        [JsonProperty("__updatedtime__")]
        public long __updatedtime__ { get; set; }

        [JsonProperty("customer_id")]
        public string customer_id { get; set; }

        [JsonProperty("ram_allocation")]
        public long ram_allocation { get; set; }

        [JsonProperty("is_local")]
        public bool is_local { get; set; }

        [JsonProperty("__createdtime__")]
        public long __createdtime__ { get; set; }

        [JsonProperty("instance_type")]
        public string instance_type { get; set; }
    }
}
