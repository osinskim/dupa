using System.Collections.Generic;
using System.Threading.Tasks;
using Smalec.Lib.Social.Enums;
using Smalec.Service.Posts.Helpers;
using Smalec.Service.Posts.Models;

namespace Smalec.Service.Posts.Abstraction
{
    public interface ISocialRepository
    {
        Task AddPost(Post post);

        Task<RawPostData> GetPostsByUserUuid(string userUuid, int page, string lastPostFetched);

        Task<RawPostData> GetMainpagePosts(int page, string lastPostFetched, IEnumerable<string> userUuids);

        Task<RawPostData> GetMyLastPost(string userUuid);

        Task AddOrUpdateReaction(Reaction reaction);

        Task<IEnumerable<Reaction>> GetReactions(IEnumerable<string> objectsUuids, ReactedObjectType objectType);
        
        Task AddComment(Comment comment);
        
        Task<IEnumerable<Comment>> GetComments(IEnumerable<string> postsUuids);
    }
}