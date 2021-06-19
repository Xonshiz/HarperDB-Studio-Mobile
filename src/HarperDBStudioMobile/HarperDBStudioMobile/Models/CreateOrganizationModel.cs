using System;
using Newtonsoft.Json;

namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Create Organization
    ///
    /// POST https://prod.harperdbcloudservices.com/addOrg
    /// </summary>
    public partial class CreateOrganizationModel
    {
        [JsonProperty("body")]
        public CreateOrganizationBody Body { get; set; }
    }

    public partial class CreateOrganizationBody
    {
        [JsonProperty("result")]
        public bool Result { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
