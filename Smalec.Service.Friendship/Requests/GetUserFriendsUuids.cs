using MediatR;
using System.Collections.Generic;

namespace Smalec.Service.Friendship.Requests
{
    public class GetUserFriendsUuids: IRequest<IEnumerable<string>>
    {
        public string CurrentUserUuid { get; set; }
    }
}
