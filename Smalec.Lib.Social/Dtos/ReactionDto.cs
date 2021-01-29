using Newtonsoft.Json;
using Smalec.Lib.Social.Enums;

namespace Smalec.Lib.Social.Dtos
{
    public class ReactionDto
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("type")]
        public ReactionType Type { get; set; }

        [JsonProperty("objectUuid")]
        public string ObjectUuid { get; set; }
    }
}