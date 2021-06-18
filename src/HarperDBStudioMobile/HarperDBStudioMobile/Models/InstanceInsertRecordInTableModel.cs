using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Roles In Instance - Insert Record In Table
    ///
    /// POST https://xontest-xonshiztestor.harperdbcloud.com/
    /// </summary>
    public partial class InstanceInsertRecordInTableModel
{
        public string Message { get; set; }
        public Guid[] InsertedHashes { get; set; }
        public object[] SkippedHashes { get; set; }
    }
}
