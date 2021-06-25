using System;
using System.Collections.Generic;

namespace HarperDBStudioMobile.Models
{
    public static class LoggedInUser
    {
        public static string BaseAuth { get; set; }
        public static string Firstname { get; set; }
        public static string Lastname { get; set; }
        public static string Email { get; set; }
        public static string LoginEmail { get; set; }
        public static string LoginPassword { get; set; }
        public static string UserId { get; set; }
        public static string PrimaryCustomerId { get; set; }
        public static object EmailBounced { get; set; }
        public static bool UpdatePassword { get; set; }
        public static object GithubRepo { get; set; }
        public static long LastLogin { get; set; }
        public static List<OrgModel> Orgs { get; set; }
        public static Dictionary<string, Dictionary<string, string>> InstanceKeyPairs = new Dictionary<string, Dictionary<string, string>>() { };
    }
}
