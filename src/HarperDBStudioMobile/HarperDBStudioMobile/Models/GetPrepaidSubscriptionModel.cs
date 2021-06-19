using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Prepaid Subscription (Basic Auth)
    ///
    /// POST https://prod.harperdbcloudservices.com/getPrepaidSubscriptions
    /// </summary>
    public partial class GetPrepaidSubscriptionModel
{
        [JsonProperty("body")]
        public GetPrepaidSubscriptionBasicAuthBody Body { get; set; }
    }

    public partial class GetPrepaidSubscriptionBasicAuthBody
    {
        [JsonProperty("cloud_compute")]
        public List<object> CloudCompute { get; set; }

        [JsonProperty("cloud_storage")]
        public List<object> CloudStorage { get; set; }

        [JsonProperty("local_compute")]
        public List<object> LocalCompute { get; set; }
    }
}
