using MediatR;
using Smalec.Lib.Social.Dtos;
using System.Collections.Generic;

namespace Smalec.ServiceFacade.Social.Requests
{
    public class GetMainpagePosts: IRequest<IEnumerable<PostDto>>
    {
        public int Page { get; set; }
        public string LastPostFetched { get; set; }
        public string CurrentUserUuid { get; set; }
    }
}
