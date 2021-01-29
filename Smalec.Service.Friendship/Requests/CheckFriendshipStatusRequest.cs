using MediatR;
using Smalec.Lib.Social.Dtos;

namespace Smalec.Service.Friendship.Requests
{
    public class CheckFriendshipStatusRequest : IRequest<FriendshipDto>
    {
        public string UserToCheck { get; set; }
        public string CurrentUser { get; set; }

        public CheckFriendshipStatusRequest(string currentUser, string userToCheck)
        {
            UserToCheck = userToCheck;
            CurrentUser = currentUser;
        }
    }
}