using System.Collections.Generic;
using Smalec.Lib.Social.Dtos;
using Smalec.Service.Posts.Helpers;

namespace Smalec.Service.Posts.Abstraction
{
    public interface IPostsBuilder
    {
        IEnumerable<PostDto> Build(RawPostData data);
    }
}