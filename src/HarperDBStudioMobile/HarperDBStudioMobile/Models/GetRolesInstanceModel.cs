using System;
using Newtonsoft.Json;

namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Roles In Instance - User Info
    ///
    /// POST https://xontest-xonshiztestor.harperdbcloud.com/
    /// </summary>
    public partial class GetRolesInstanceModel
    {
        [JsonProperty("__createdtime__")]
        public long __createdtime__ { get; set; }

        [JsonProperty("__updatedtime__")]
        public long __updatedtime__ { get; set; }

        [JsonProperty("active")]
        public bool active { get; set; }

        [JsonProperty("role")]
        public Role role { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }
    }

    public partial class Role
    {
        [JsonProperty("__createdtime__")]
        public long __createdtime__ { get; set; }

        [JsonProperty("__updatedtime__")]
        public long __updatedtime__ { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("permission")]
        public RolePermission permission { get; set; }

        [JsonProperty("role")]
        public string role { get; set; }
    }

    public partial class RolePermission
    {
        [JsonProperty("super_user")]
        public bool super_user { get; set; }
    }
}
