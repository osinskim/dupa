using Smalec.Lib.Social.Dtos;
using System.Collections.Generic;

namespace Smalec.Service.Posts.Helpers
{
    public class RawPostData
    {
        public IEnumerable<PostDto> Posts { get; set; }
        public IEnumerable<ReactionDto> PostsReactions { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
        public IEnumerable<ReactionDto> CommentsReactions { get; set; }
    }
}