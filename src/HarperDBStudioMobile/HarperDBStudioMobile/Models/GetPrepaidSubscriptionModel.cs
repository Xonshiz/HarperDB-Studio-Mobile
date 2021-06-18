using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Prepaid Subscription (Basic Auth)
    ///
    /// POST https://prod.harperdbcloudservices.com/getPrepaidSubscriptions
    /// </summary>
    public partial class GetPrepaidSubscriptionModel
{
        public GetPrepaidSubscriptionBasicAuthBody Body { get; set; }
    }

    public partial class GetPrepaidSubscriptionBasicAuthBody
    {
        public object[] CloudCompute { get; set; }
        public object[] CloudStorage { get; set; }
        public object[] LocalCompute { get; set; }
    }
}
