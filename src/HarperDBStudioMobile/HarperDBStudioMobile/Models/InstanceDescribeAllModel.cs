using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Roles In Instance - Descrbe All
    ///
    /// POST https://xontest-xonshiztestor.harperdbcloud.com/
    /// </summary>
    public partial class InstanceDescribeAllModel
{
        public NewSchema NewSchema { get; set; }
        public NewSchema2 NewSchema2 { get; set; }
    }

    public partial class NewSchema
    {
        public NewTable NewTable { get; set; }
        public NewTable NewTable2 { get; set; }
    }

    public partial class NewTable
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

    public partial class Attribute
    {
        public string AttributeAttribute { get; set; }
    }

    public partial class NewSchema2
    {
    }
}
