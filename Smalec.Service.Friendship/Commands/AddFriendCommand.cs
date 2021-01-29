using MediatR;

namespace Smalec.Service.Friendship.Commands
{
    public class AddFriendCommand : INotification
    {
        public string CurrentUser { get; set; }

        public string UserToAdd { get; set; }
    }
}