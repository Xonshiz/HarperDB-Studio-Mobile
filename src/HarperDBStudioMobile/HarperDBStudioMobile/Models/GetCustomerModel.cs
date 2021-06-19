using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Customer (Requires Auth)
    ///
    /// POST https://prod.harperdbcloudservices.com/getCustomer
    /// </summary>
    public partial class GetCustomerModel
    {
        public GetCustomerRequiresAuthBody Body { get; set; }
    }

    public partial class GetCustomerRequiresAuthBody
    {
        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }

        [JsonProperty("stripe_id")]
        public string StripeId { get; set; }

        [JsonProperty("hubspot_company_id")]
        public string HubspotCompanyId { get; set; }

        [JsonProperty("coupon_id")]
        public List<object> CouponId { get; set; }

        [JsonProperty("removed_by")]
        public object RemovedBy { get; set; }

        [JsonProperty("creation_date")]
        public long CreationDate { get; set; }

        [JsonProperty("current_payment_status")]
        public CurrentPaymentStatus CurrentPaymentStatus { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("owner_user_id")]
        public Guid OwnerUserId { get; set; }

        [JsonProperty("subdomain")]
        public string Subdomain { get; set; }

        [JsonProperty("tc_version")]
        public DateTimeOffset TcVersion { get; set; }

        [JsonProperty("stripe_payment_methods")]
        public List<object> StripePaymentMethods { get; set; }

        [JsonProperty("stripe_coupons")]
        public List<object> StripeCoupons { get; set; }

        [JsonProperty("stripe_customer_balance_transactions")]
        public StripeCustomerBalanceTransactions StripeCustomerBalanceTransactions { get; set; }
    }

    public partial class CurrentPaymentStatus
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("invoice_id")]
        public string InvoiceId { get; set; }

        [JsonProperty("ending_balance")]
        public long EndingBalance { get; set; }
    }

    public partial class StripeCustomerBalanceTransactions
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("data")]
        public List<object> Data { get; set; }

        [JsonProperty("has_more")]
        public bool HasMore { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
