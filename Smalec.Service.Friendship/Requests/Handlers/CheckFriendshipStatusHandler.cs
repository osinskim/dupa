using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Lib.Social.Dtos;
using Smalec.Lib.Social.Enums;
using Smalec.Service.Friendship.Abstraction;

namespace Smalec.Service.Friendship.Requests.Handlers
{
    public class CheckFriendshipStatusHandler : IRequestHandler<CheckFriendshipStatusRequest, FriendshipDto>
    {
        private readonly IFriendshipRepository _repo;

        public CheckFriendshipStatusHandler(IFriendshipRepository repo)
        {
            _repo = repo;
        }

        public async Task<FriendshipDto> Handle(CheckFriendshipStatusRequest request, CancellationToken cancellationToken)
        {
            var friendshipInfo = await _repo.GetFriendshipInfo(request.CurrentUser, request.UserToCheck);
            var result = new FriendshipDto();
            
            if (friendshipInfo == null)
                result.Status = FriendshipStatus.UNDEFINED;
            else if (friendshipInfo.IsAccepted)
                result.Status = FriendshipStatus.ACCEPTED;
            else if (friendshipInfo.IsAccepted == false && string.Equals(friendshipInfo.InvitationSentBy, request.CurrentUser))
                result.Status = FriendshipStatus.PENDING_SENT;
            else if (friendshipInfo.IsAccepted == false && string.Equals(friendshipInfo.InvitationSentBy, request.UserToCheck))
                result.Status = FriendshipStatus.PENDING_RECEIVED;

            return result;
        }
    }
}