using Newtonsoft.Json;

namespace Smalec.Lib.Social.Dtos
{
    public class AddCommentDto
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("postUuid")]
        public string PostUuid { get; set; }

        [JsonProperty("postOwnerUuid")]
        public string PostOwnerUuid { get; set; }
    }
}
