using System.Net.Http;
using System.Threading.Tasks;

namespace Smalec.Lib.Shared.Abstraction.Interfaces
{
    public interface IConsulHttpClient
    {
        Task<string> Post(string serviceName, string resourcePath, HttpContent content);

        Task<string> Get(string serviceName, string resourcePath, string urlQuery);
    }
}
