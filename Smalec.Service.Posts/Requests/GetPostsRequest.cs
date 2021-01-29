using System.Collections.Generic;
using MediatR;
using Smalec.Lib.Social.Dtos;
using Smalec.Lib.Social.Enums;

namespace Smalec.Service.Posts.Requests
{
    public class GetPostsRequest : IRequest<IEnumerable<PostDto>>
    {
        public string CurrentUserUuid { get; set; }

        public IEnumerable<string> FriendsUuid { get; set; }

        public PostDestination Destination { get; set; }

        public int Page { get; set; }

        public string LastPostFetched { get; set; }
    }
}