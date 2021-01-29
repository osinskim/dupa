using System.Collections.Generic;
using System.Linq;
using Smalec.Lib.Social.Dtos;
using Smalec.Service.Posts.Abstraction;
using Smalec.Service.Posts.Helpers;

namespace Smalec.Service.Posts.Builders.Posts
{
    public class PostsBuilder : IPostsBuilder
    {
        public IEnumerable<PostDto> Build(RawPostData data)
        {
            MergeCommentsWithReactions(data.Comments, data.CommentsReactions);
            MergePostsWithReactions(data.Posts, data.PostsReactions);
            MergePostsWithComments(data.Posts, data.Comments);

            return data.Posts;
        }

        private void MergePostsWithReactions(IEnumerable<PostDto> posts, IEnumerable<ReactionDto> reactions)
        {
            var  reactionsForPosts = reactions.GroupBy(x => x.ObjectUuid);

            foreach(var reactionsForPost in reactionsForPosts)
            {
                var post = posts.Where(x => x.Uuid == reactionsForPost.Key).FirstOrDefault();

                if(post == null)
                    continue;
                
                post.Reactions = reactionsForPost;
            }
        }
        
        private void MergeCommentsWithReactions(IEnumerable<CommentDto> comments, IEnumerable<ReactionDto> commentsReactions)
        {
            var  reactionsForComments = commentsReactions.GroupBy(x => x.ObjectUuid);

            foreach(var reactionsForComment in reactionsForComments)
            {
                var comment = comments.Where(x => x.Uuid == reactionsForComment.Key).FirstOrDefault();

                if(comment == null)
                    continue;
                
                comment.Reactions = reactionsForComment;
            }
        }

        private void MergePostsWithComments(IEnumerable<PostDto> posts, IEnumerable<CommentDto> comments)
        {
            var  commentsForPosts = comments.GroupBy(x => x.PostUuid);

            foreach(var commentsForPost in commentsForPosts)
            {
                var post = posts.FirstOrDefault(x => string.Equals(x.Uuid, commentsForPost.Key));

                if(post == null)
                    continue;
                
                post.Comments = commentsForPost;
            }
        }
    }
}