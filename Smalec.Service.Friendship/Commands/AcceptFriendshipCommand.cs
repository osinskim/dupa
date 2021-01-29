using MediatR;

namespace Smalec.Service.Friendship.Commands
{
    public class AcceptFriendshipCommand : INotification
    {
        public string CurrentUser { get; set; }

        public string UserToAccept { get; set; }
    }
}