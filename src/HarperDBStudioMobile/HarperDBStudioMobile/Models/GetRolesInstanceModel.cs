using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Roles In Instance - User Info
    ///
    /// POST https://xontest-xonshiztestor.harperdbcloud.com/
    /// </summary>
    public partial class GetRolesInstanceModel
{
        public long Createdtime { get; set; }
        public long Updatedtime { get; set; }
        public bool Active { get; set; }
        public Role Role { get; set; }
        public string Username { get; set; }
    }

    public partial class Role
    {
        public long Createdtime { get; set; }
        public long Updatedtime { get; set; }
        public Guid Id { get; set; }
        public RolePermission Permission { get; set; }
        public string RoleRole { get; set; }
    }

    public partial class RolePermission
    {
        public bool SuperUser { get; set; }
    }
}
