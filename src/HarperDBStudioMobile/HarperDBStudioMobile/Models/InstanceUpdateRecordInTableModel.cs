using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Roles In Instance - Update Table Record
    ///
    /// POST https://xontest-xonshiztestor.harperdbcloud.com/
    /// </summary>
    public partial class InstanceUpdateRecordInTableModel
{
        public string Message { get; set; }
        public object[] UpdateHashes { get; set; }
        public Guid[] SkippedHashes { get; set; }
    }

    public partial class GetRolesInInstanceGetRecordDetails
    {
        public long Createdtime { get; set; }
        public long Updatedtime { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Guid RecId { get; set; }
    }

}
