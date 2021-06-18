using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Roles In Instance - Registration Info
    ///
    /// POST https://xontest-xonshiztestor.harperdbcloud.com/
    /// </summary>
    public partial class InstanceGetRolesModel
{
        public bool Registered { get; set; }
        public string Version { get; set; }
        public string StorageType { get; set; }
        public long RamAllocation { get; set; }
        public DateTimeOffset LicenseExpirationDate { get; set; }
    }
}
