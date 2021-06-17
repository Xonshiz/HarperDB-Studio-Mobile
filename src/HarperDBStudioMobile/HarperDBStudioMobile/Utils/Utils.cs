using System;
namespace HarperDBStudioMobile.Utils
{
    public static class Utils
    {
        public static string BASE_API_URL = "https://prod.harperdbcloudservices.com";
        public static string MAIN_BASIC_AUTH { get; set; }
        public static string INSTANCE_BASIC_AUTH { get; set; }
        public enum TABLE_RECORD_MODES
        {
            Edit,
            Add
        }
    }
}
