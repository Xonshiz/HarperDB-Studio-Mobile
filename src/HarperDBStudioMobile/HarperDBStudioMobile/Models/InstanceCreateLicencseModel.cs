using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Roles In Instance - Create License
    ///
    /// POST https://prod.harperdbcloudservices.com/createLicense
    /// </summary>
    public partial class InstanceCreateLicencseModel
    {
        public AddInstanceBody Body { get; set; }
    }

    /// <summary>
    /// Get Roles In Instance - Set License
    ///
    /// POST https://xontest-xonshiztestor.harperdbcloud.com/
    /// </summary>
    public partial class InstanceSetLicense
    {
        public string Message { get; set; }
    }
}
