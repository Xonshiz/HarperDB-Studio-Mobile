using System.Threading.Tasks;
using HarperDBStudioMobile.Models;
using Refit;

namespace HarperDBStudioMobile.Interfaces
{
    [Headers("User-Agent: :request:")]
    public interface IRestClient
    {
        [Post("/getUser")]
        Task<ApiResponse<GetUserModel>> GetUser([Body] RequestGetUserModel body);

        [Post("/6")]
        Task<StripeCallModel> StripeCall([Body] string body);

        [Post("/addOrg")]
        Task<OrgModel> CreatOrg([Body] RequestCreateOrganizationModel body);

        [Post("/getCustomer")]
        Task<GetCustomerModel> GetCustomer([Body] RequestGetCustomerModel body);

        [Post("/getPrepaidSubscriptions")]
        Task<GetPrepaidSubscriptionModel> GetPrepaidSubscriptions([Body] RequestGetPrepaidSubscriptionModel body);

        [Post("/getInstances")]
        Task<ApiResponse<GetInstancesModel>> GetInstances([Header("Authorization")] string authString, [Body] RequestGetInstancesModel body);

        [Post("/getAlarms")]
        Task<GetAlarmsModel> GetAlarms([Body] RequestGetAlarmsModel body );

        [Post("/addTCAcceptance")]
        Task<AddAtcAcceptanceModel> AddTCAcceptance([Body] RequestAddTCAcceptanceModel body);

        [Post("/addInstance")]
        Task<InstanceAddModel> AddInstance([Body] RequestAddInstanceModel body);

        [Post("/addError")]
        Task<AddErrorModel> AddError([Body] RequestAddErrorModel body);
    }
}
