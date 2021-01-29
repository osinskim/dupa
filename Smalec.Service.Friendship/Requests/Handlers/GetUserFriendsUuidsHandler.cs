using MediatR;
using Smalec.Service.Friendship.Abstraction;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Smalec.Service.Friendship.Requests.Handlers
{
    public class GetUserFriendsUuidsHandler : IRequestHandler<GetUserFriendsUuids, IEnumerable<string>>
    {
        private readonly IFriendshipRepository _repo;

        public GetUserFriendsUuidsHandler(IFriendshipRepository repo)
        {
            _repo = repo;
        }


        public async Task<IEnumerable<string>> Handle(GetUserFriendsUuids request, CancellationToken cancellationToken)
        {
            return await _repo.GetUserFriendsUuids(request.CurrentUserUuid);
        }
    }
}
