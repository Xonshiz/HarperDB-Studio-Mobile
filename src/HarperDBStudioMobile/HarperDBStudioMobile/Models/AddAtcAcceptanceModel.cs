using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Add ATC Acceptance
    ///
    /// POST https://prod.harperdbcloudservices.com/addTCAcceptance
    /// </summary>
    public partial class AddAtcAcceptanceModel
{
        public AddAtcAcceptanceBody Body { get; set; }
    }

    public partial class AddAtcAcceptanceBody
    {
        public string Message { get; set; }
        public bool Result { get; set; }
    }
}
