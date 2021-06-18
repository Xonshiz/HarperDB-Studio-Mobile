using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Roles In Instance - Delete Record
    ///
    /// POST https://xontest-xonshiztestor.harperdbcloud.com/
    /// </summary>
    public partial class InstanceDeleteRecordInTableModel
{
        public string Message { get; set; }
        public object[] DeletedHashes { get; set; }
        public Guid[] SkippedHashes { get; set; }
    }

    public partial class GetRolesInInstanceGetAllRecordsInTable
    {
        public long Updatedtime { get; set; }
        public string Name { get; set; }
        public long Createdtime { get; set; }
        public string Email { get; set; }
        public Guid RecId { get; set; }
    }
}
