using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Log In
    ///
    /// POST https://prod.harperdbcloudservices.com/getUser
    /// </summary>
    public class GetUserModel
    {
        public LogInBody Body { get; set; }
    }

    public partial class LogInBody
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public string PrimaryCustomerId { get; set; }
        public object EmailBounced { get; set; }
        public bool UpdatePassword { get; set; }
        public object GithubRepo { get; set; }
        public long LastLogin { get; set; }
        public OrgModel[] Orgs { get; set; }
    }
}
