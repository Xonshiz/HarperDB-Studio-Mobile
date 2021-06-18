using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Roles In Instance - Describe Table
    ///
    /// POST https://xontest-xonshiztestor.harperdbcloud.com/
    /// </summary>
    public partial class InstanceDescribeTableModel
{
        public long Createdtime { get; set; }
        public long Updatedtime { get; set; }
        public string HashAttribute { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public object Residence { get; set; }
        public string Schema { get; set; }
        public Attribute[] Attributes { get; set; }
        public long RecordCount { get; set; }
    }
}
