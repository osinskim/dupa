using System.Threading.Tasks;
using Smalec.Service.Posts.Helpers;
using Smalec.Service.Posts.Requests;

namespace Smalec.Service.Posts.Abstraction
{
    public interface IGetPostsStrategy
    {
        bool CanGet(GetPostsRequest request);

        Task<RawPostData> GetRawPostsData(GetPostsRequest request);
    }
}