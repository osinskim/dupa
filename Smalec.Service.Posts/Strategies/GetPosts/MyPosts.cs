using System.Threading.Tasks;
using Smalec.Lib.Social.Enums;
using Smalec.Service.Posts.Abstraction;
using Smalec.Service.Posts.Helpers;
using Smalec.Service.Posts.Requests;

namespace Smalec.Service.Posts.Strategies.GetPosts
{
    public class MyPosts : IGetPostsStrategy
    {
        private readonly ISocialRepository _repo;

        public MyPosts(ISocialRepository repo)
        {
            _repo = repo;
        }

        public bool CanGet(GetPostsRequest request)
        {
            return request.Destination == PostDestination.MyProfile;
        }

        public async Task<RawPostData> GetRawPostsData(GetPostsRequest request)
        {
            return await _repo.GetPostsByUserUuid(request.CurrentUserUuid, request.Page, request.LastPostFetched);
        }
    }
}