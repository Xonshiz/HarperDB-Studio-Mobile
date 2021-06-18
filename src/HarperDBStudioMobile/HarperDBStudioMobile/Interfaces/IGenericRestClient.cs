using System.Threading.Tasks;
using Refit;

namespace HarperDBStudioMobile.Interfaces
{
    [Headers("User-Agent: :request:")]
    public interface IGenericRestClient<T, in TKey> where T : class
    {
        //var api = RestService.For<IReallyExcitingCrudApi<User, string>>("http://api.example.com/users");
        [Post("/")]
        Task<T> InstanceCall([Body] TKey body);
    }
}
