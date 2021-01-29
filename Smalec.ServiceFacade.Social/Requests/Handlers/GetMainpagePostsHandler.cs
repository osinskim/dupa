using MediatR;
using Newtonsoft.Json;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Social.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smalec.ServiceFacade.Social.Requests.Handlers
{
    public class GetMainpagePostsHandler : IRequestHandler<GetMainpagePosts, IEnumerable<PostDto>>
    {
        private readonly IConsulHttpClient _http;

        public GetMainpagePostsHandler(IConsulHttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetMainpagePosts request, CancellationToken cancellationToken)
        {
            var myFriendsUuids = await GetMyFriends(request.CurrentUserUuid);

            return await GetPosts(myFriendsUuids, request);
        }

        private async Task<IEnumerable<string>> GetMyFriends(string currentUserUuid)
        {
            var result = await _http.Get("friendship", "/GetUserFriendsUuids", "currentUserUuid=" + currentUserUuid);

            return JsonConvert.DeserializeObject<string[]>(result);
        }

        private async Task<IEnumerable<PostDto>> GetPosts(IEnumerable<string> myFriendsUuids, GetMainpagePosts request)
        {
            var dto = new GetMainpagePostsDto
            {
                LastPostFetched = request.LastPostFetched,
                Page = request.Page,
                MyFriendsUuids = myFriendsUuids
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json");

            var result = await _http.Post("posts", "/GetMainpagePosts", content);

            return JsonConvert.DeserializeObject<IEnumerable<PostDto>>(result);
        }
    }
}
