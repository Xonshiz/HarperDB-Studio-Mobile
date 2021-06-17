using System;
using System.Threading.Tasks;
using Refit;

namespace HarperDBStudioMobile.Interfaces
{
    [Headers("User-Agent: :request:")]
    public interface IRestClient
    {
        [Post("/getUser")]
        Task<string> GetUser([Body] string body);
    }
}
