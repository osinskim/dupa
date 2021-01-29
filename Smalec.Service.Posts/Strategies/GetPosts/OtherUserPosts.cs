using System;
using System.Linq;
using System.Threading.Tasks;
using Smalec.Lib.Social.Enums;
using Smalec.Service.Posts.Abstraction;
using Smalec.Service.Posts.Helpers;
using Smalec.Service.Posts.Requests;

namespace Smalec.Service.Posts.Strategies.GetPosts
{
    public class OtherUserPosts : IGetPostsStrategy
    {
        private readonly ISocialRepository _repo;

        public OtherUserPosts(ISocialRepository repo)
        {
            _repo = repo;
        }

        public bool CanGet(GetPostsRequest request)
        {
            return request.Destination == PostDestination.OtherUserProfile;
        }

        public async Task<RawPostData> GetRawPostsData(GetPostsRequest request)
        {
            var userUuid = string.IsNullOrEmpty(request.FriendsUuid.FirstOrDefault())
                ? Guid.Empty.ToString()
                : request.FriendsUuid.FirstOrDefault();

            return await _repo.GetPostsByUserUuid(userUuid, request.Page, request.LastPostFetched);
        }
    }
}