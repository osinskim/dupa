using System;

namespace Smalec.Lib.Shared.Exceptions
{
    public class FriendshipException : Exception
    {
        public FriendshipException() : base("Friendship status check failure.")
        { }
    }
}
