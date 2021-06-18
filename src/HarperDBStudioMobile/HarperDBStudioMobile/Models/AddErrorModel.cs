using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Add Error
    ///
    /// POST https://prod.harperdbcloudservices.com/addError
    /// </summary>
    public partial class AddErrorModel
{
        public string Message { get; set; }
    }

    public partial class GetRolesInInstance
    {
        public long Createdtime { get; set; }
        public long Updatedtime { get; set; }
        public Guid Id { get; set; }
        public GetRolesInInstancePermission Permission { get; set; }
        public string Role { get; set; }
    }

    public partial class GetRolesInInstancePermission
    {
        public bool? SuperUser { get; set; }
        public bool? ClusterUser { get; set; }
    }
}
