﻿using System;
using System.Collections.Generic;

namespace HarperDBStudioMobile.Models
{
    public static class LoggedInUser
    {
        public static string Firstname { get; set; }
        public static string Lastname { get; set; }
        public static string Email { get; set; }
        public static string UserId { get; set; }
        public static string PrimaryCustomerId { get; set; }
        public static object EmailBounced { get; set; }
        public static bool UpdatePassword { get; set; }
        public static object GithubRepo { get; set; }
        public static long LastLogin { get; set; }
        public static List<OrgModel> Orgs { get; set; }
    }
}
