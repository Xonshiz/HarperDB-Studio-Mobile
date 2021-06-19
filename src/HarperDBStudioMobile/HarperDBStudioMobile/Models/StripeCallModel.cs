using System;
using Newtonsoft.Json;

namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Random Stripe Call
    ///
    /// POST https://m.stripe.com/6
    /// </summary>
    public partial class StripeCallModel
    {
        [JsonProperty("muid")]
        public string Muid { get; set; }

        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("sid")]
        public string Sid { get; set; }
    }
}
