using MediatR;

namespace Smalec.Service.Friendship.Commands
{
    public class RejectFriendshipCommand : INotification
    {
        public string CurrentUser { get; set; }

        public string UserToReject { get; set; }
    }
}