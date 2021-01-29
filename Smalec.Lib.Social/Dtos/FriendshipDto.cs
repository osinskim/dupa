using Newtonsoft.Json;
using Smalec.Lib.Social.Enums;

namespace Smalec.Lib.Social.Dtos
{
    public class FriendshipDto
    {
        [JsonProperty("status")]
        public FriendshipStatus Status { get; set; }
    }
}