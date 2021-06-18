using System;
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
        public string CustomerName { get; set; }
        public string StripeId { get; set; }
        public string HubspotCompanyId { get; set; }
        public object[] CouponId { get; set; }
        public object RemovedBy { get; set; }
        public long CreationDate { get; set; }
        public CurrentPaymentStatus CurrentPaymentStatus { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public Guid OwnerUserId { get; set; }
        public string Subdomain { get; set; }
        public DateTimeOffset TcVersion { get; set; }
        public object[] StripePaymentMethods { get; set; }
        public object[] StripeCoupons { get; set; }
        public StripeCustomerBalanceTransactions StripeCustomerBalanceTransactions { get; set; }
    }

    public partial class CurrentPaymentStatus
    {
        public string Status { get; set; }
        public DateTimeOffset Date { get; set; }
        public string InvoiceId { get; set; }
        public long EndingBalance { get; set; }
    }

    public partial class StripeCustomerBalanceTransactions
    {
        public string Object { get; set; }
        public object[] Data { get; set; }
        public bool HasMore { get; set; }
        public string Url { get; set; }
    }
}
