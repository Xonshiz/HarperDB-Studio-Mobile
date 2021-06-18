using System;
namespace HarperDBStudioMobile.Models
{
    public partial class OrgModel
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public Guid OwnerUserId { get; set; }
        public long TotalInstanceCount { get; set; }
        public long FreeCloudInstanceCount { get; set; }
        public object[] ActiveCoupons { get; set; }
    }
}
