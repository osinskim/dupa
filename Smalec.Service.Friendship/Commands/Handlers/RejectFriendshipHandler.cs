using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Service.Friendship.Abstraction;

namespace Smalec.Service.Friendship.Commands.Handlers
{
    public class RejectFriendshipHandler : INotificationHandler<RejectFriendshipCommand>
    {
        private readonly IFriendshipRepository _repo;
        
        public RejectFriendshipHandler(IFriendshipRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(RejectFriendshipCommand command, CancellationToken cancellationToken)
        {
            if(command.CurrentUser == command.UserToReject)
                return;
                
            await _repo.RejectFriend(command.CurrentUser, command.UserToReject);
        }
    }
}