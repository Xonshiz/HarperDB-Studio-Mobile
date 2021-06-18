using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Create Organization
    ///
    /// POST https://prod.harperdbcloudservices.com/addOrg
    /// </summary>
    public partial class CreateOrganizationModel
    {
        public CreateOrganizationBody Body { get; set; }
    }

    public partial class CreateOrganizationBody
    {
        public bool Result { get; set; }
        public string CustomerId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
    }
}
