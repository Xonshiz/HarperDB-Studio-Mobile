using System.Threading.Tasks;
using HarperDBStudioMobile.Models;
using Refit;

namespace HarperDBStudioMobile.Interfaces
{
    [Headers("User-Agent: :request:")]
    public interface IGenericRestClient<T, in TKey> where T : class
    {
        //var api = RestService.For<IReallyExcitingCrudApi<User, string>>("http://api.example.com/users");
        [Post("/")]
        Task<ApiResponse<T>> InstanceCall([Header("Authorization")] string authString, [Body] TKey body);
    }
}
