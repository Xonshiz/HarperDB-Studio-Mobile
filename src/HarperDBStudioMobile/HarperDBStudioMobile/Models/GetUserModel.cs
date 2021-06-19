using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Log In
    ///
    /// POST https://prod.harperdbcloudservices.com/getUser
    /// </summary>
    public class GetUserModel
    {
        [JsonProperty("body")]
        public LogInBody Body { get; set; }
    }

    public class LogInBody
    {
        [JsonProperty("firstname")]
        public string firstname { get; set; }

        [JsonProperty("lastname")]
        public string lastname { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("user_id")]
        public string user_id { get; set; }

        [JsonProperty("primary_customer_id")]
        public string primary_customer_id { get; set; }

        [JsonProperty("email_bounced")]
        public object email_bounced { get; set; }

        [JsonProperty("update_password")]
        public bool update_password { get; set; }

        [JsonProperty("github_repo")]
        public object github_repo { get; set; }

        [JsonProperty("last_login")]
        public long last_login { get; set; }

        [JsonProperty("orgs")]
        public List<OrgModel> orgs { get; set; }
    }
}
