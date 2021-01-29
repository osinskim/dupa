using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Service.Friendship.Abstraction;

namespace Smalec.Service.Friendship.Commands.Handlers
{
    public class AddFriendHandler : INotificationHandler<AddFriendCommand>
    {
        private readonly IFriendshipRepository _repo;
        
        public AddFriendHandler(IFriendshipRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(AddFriendCommand command, CancellationToken cancellationToken)
        {
            if(command.CurrentUser == command.UserToAdd)
                return;
                
            await _repo.AddFriend(command.CurrentUser, command.UserToAdd);
        }
    }
}