using MediatR;
using Newtonsoft.Json;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.Exceptions;
using Smalec.Lib.Social.Dtos;
using Smalec.Lib.Social.Enums;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smalec.ServiceFacade.Social.Commands.Handlers
{
    public class AddCommentHandler : INotificationHandler<AddComment>
    {
        private readonly IConsulHttpClient _http;

        public AddCommentHandler(IConsulHttpClient http)
        {
            _http = http;
        }

        public async Task Handle(AddComment notification, CancellationToken cancellationToken)
        {
            if (string.Equals(notification.CurrentUserUuid, notification.PostOwnerUuid)
                || await IsMyFriend(notification.PostOwnerUuid))
            {
                await AddComment(notification);
            }
            else
            {
                throw new FriendshipException();
            }
        }

        private async Task<bool> IsMyFriend(string postOwnerUuid)
        {
            var friendshipStatusDto = await _http.Get(
                "friendship",
                "/CheckStatus",
                "userToCheck=" + postOwnerUuid);

            var friendshipStatus = JsonConvert.DeserializeObject<FriendshipDto>(friendshipStatusDto);

            return friendshipStatus.Status == FriendshipStatus.ACCEPTED;
        }

        private async Task AddComment(AddComment notification)
        {
            var dto = new AddCommentDto
            {
                Text = notification.Text,
                PostOwnerUuid = notification.PostOwnerUuid,
                PostUuid = notification.PostUuid
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json");

            await _http.Post("posts", "/AddComment", content);
        }
    }
}
