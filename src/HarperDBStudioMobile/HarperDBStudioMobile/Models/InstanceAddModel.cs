using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Add Instance
    ///
    /// POST https://prod.harperdbcloudservices.com/v2/addInstance
    /// </summary>
    public partial class InstanceAddModel
{
        public AddInstanceBody Body { get; set; }
    }

    public partial class AddInstanceBody
    {
        public string Message { get; set; }
        public bool Error { get; set; }
    }
}
