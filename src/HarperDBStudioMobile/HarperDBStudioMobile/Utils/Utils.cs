﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarperDBStudioMobile.Utils
{
    public static class Utils
    {
        public static string BASE_API_URL = "https://prod.harperdbcloudservices.com";
        public static string BASE_STRIPE_URL = "https://m.stripe.com";
        public static string HARPERDB_INSTANCE_INTIAL_ROOT_OWNER = "";
        public static string BASE_BASIC_AUTH { get; set; }
        public static string INSTANCE_BASIC_AUTH { get; set; }
        public enum STORAGE_KEYS
        {
            BASE_EMAIL,
            BASE_PASSWORD,
            LOGGED_IN_USER_ORGS,
            INSTANCE_MODELS,
            INSTANCE_CREDENTIALS,
            SCHEMA_DATA,
            CACHED_OPERATIONS
        }
        public enum TABLE_RECORD_MODES
        {
            Edit,
            Add
        }
        public enum INSTANCE_OPERATIONS
        {
            list_roles,
            user_info,
            registration_info,
            get_fingerprint,
            restart,
            describe_all,
            add_user,
            drop_user,
            set_license,
            create_schema,
            create_table,
            describe_table,
            insert,
            update,
            search_by_hash,
            delete,
            sql
        }

        public static string GetBasicAuthString(string username, string password)
        {
            var authData = string.Format("{0}:{1}", username, password);
            return $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(authData))}";
        }
    }
}
