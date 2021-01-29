using System.Collections.Generic;

namespace Smalec.Lib.Social.Dtos
{
    public class GetMainpagePostsDto
    {
        public int Page { get; set; }
        public string LastPostFetched { get; set; }
        public IEnumerable<string> MyFriendsUuids { get; set; }
    }
}
