using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Random Stripe Call
    ///
    /// POST https://m.stripe.com/6
    /// </summary>
    public partial class StripeCallModel
    {
        public string Muid { get; set; }
        public string Guid { get; set; }
        public string Sid { get; set; }
    }
}
