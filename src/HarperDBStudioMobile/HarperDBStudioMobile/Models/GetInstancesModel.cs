using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Instances
    ///
    /// POST https://prod.harperdbcloudservices.com/getInstances
    /// </summary>
    public partial class GetInstancesModel
{
        public BodyElement[] Body { get; set; }
    }

    public partial class BodyElement
    {
        public string RuleArn { get; set; }
        public object Port { get; set; }
        public string InstanceTier { get; set; }
        public string StripeStoragePlanId { get; set; }
        public object StorageSubscriptionId { get; set; }
        public string ComputeStackId { get; set; }
        public string ClusteringTargetGroupArn { get; set; }
        public string StripePlanId { get; set; }
        public string ClusteringRuleArn { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public bool IsSsl { get; set; }
        public string StorageSubItemId { get; set; }
        public string NetworkStackId { get; set; }
        public string Status { get; set; }
        public string ComputeSubItemId { get; set; }
        public string PrivateIp { get; set; }
        public Uri Url { get; set; }
        public object Host { get; set; }
        public string InstanceName { get; set; }
        public object ComputeSubscriptionId { get; set; }
        public string InstanceId { get; set; }
        public object LastVolumeResize { get; set; }
        public string StripeSubscriptionId { get; set; }
        public string TargetGroupArn { get; set; }
        public string InstanceRegion { get; set; }
        public string ComputeConnectStackId { get; set; }
        public long DataVolumeSize { get; set; }
        public object Hidden { get; set; }
        public long Updatedtime { get; set; }
        public string CustomerId { get; set; }
        public long RamAllocation { get; set; }
        public bool IsLocal { get; set; }
        public long Createdtime { get; set; }
        public string InstanceType { get; set; }
    }
}
