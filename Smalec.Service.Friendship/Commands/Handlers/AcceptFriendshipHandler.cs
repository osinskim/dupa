using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Service.Friendship.Abstraction;

namespace Smalec.Service.Friendship.Commands.Handlers
{
    public class AcceptFriendshipHandler : INotificationHandler<AcceptFriendshipCommand>
    {
        private readonly IFriendshipRepository _repo;
        
        public AcceptFriendshipHandler(IFriendshipRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(AcceptFriendshipCommand command, CancellationToken cancellationToken)
        {
            if(command.CurrentUser == command.UserToAccept)
                return;
                
            await _repo.AcceptFriend(command.CurrentUser, command.UserToAccept);
        }
    }
}