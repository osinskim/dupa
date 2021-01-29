using System.Threading.Tasks;
using Smalec.Lib.Social.Enums;
using Smalec.Service.Posts.Abstraction;
using Smalec.Service.Posts.Helpers;
using Smalec.Service.Posts.Requests;

namespace Smalec.Service.Posts.Strategies.GetPosts
{
    public class MyNewlyAddedPost : IGetPostsStrategy
    {
        private readonly ISocialRepository _repo;

        public MyNewlyAddedPost(ISocialRepository repo)
        {
            _repo = repo;
        }

        public bool CanGet(GetPostsRequest request)
        {
            return request.Destination == PostDestination.MyNewlyAddedPost;
        }

        public async Task<RawPostData> GetRawPostsData(GetPostsRequest request)
        {
            return await _repo.GetMyLastPost(request.CurrentUserUuid);
        }
    }
}