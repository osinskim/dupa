using System.Linq;
using System.Threading.Tasks;
using Smalec.Lib.Social.Enums;
using Smalec.Service.Posts.Abstraction;
using Smalec.Service.Posts.Helpers;
using Smalec.Service.Posts.Requests;

namespace Smalec.Service.Posts.Strategies.GetPosts
{
    public class MainpagePosts : IGetPostsStrategy
    {
        private readonly ISocialRepository _repo;

        public MainpagePosts(ISocialRepository repo)
        {
            _repo = repo;
        }

        public bool CanGet(GetPostsRequest request)
        {
            return request.Destination == PostDestination.MainPage;
        }

        public async Task<RawPostData> GetRawPostsData(GetPostsRequest request)
        {
            var userUuids = request.FriendsUuid.Append(request.CurrentUserUuid);

            return await _repo.GetMainpagePosts(request.Page, request.LastPostFetched, userUuids);
        }
    }
}