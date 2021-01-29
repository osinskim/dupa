using Smalec.Service.Friendship.Models.Relations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smalec.Service.Friendship.Abstraction
{
    public interface IFriendshipRepository
    {
        Task RejectFriend(string currentUser, string userToReject);
        Task AcceptFriend(string currentUser, string userToAccept);
        Task AddFriend(string currentUser, string userToAdd);
        Task<FRIENDS_WITH> GetFriendshipInfo(string currentUser, string userToCheck);
        Task<IEnumerable<string>> GetUserFriendsUuids(string currentUserUuid);
    }
}
