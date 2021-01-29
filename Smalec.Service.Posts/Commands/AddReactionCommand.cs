using MediatR;
using Newtonsoft.Json;
using Smalec.Lib.Social.Enums;

namespace Smalec.Service.Posts.Commands
{
    public class AddReactionCommand : INotification
    {
        [JsonIgnore]
        public string UserUuid { get; set; }

        [JsonProperty("reaction")]
        public ReactionType Reaction { get; set; }

        [JsonProperty("objectUuid")]
        public string ObjectUuid { get; set; }

        [JsonProperty("reactedObjectType")]
        public ReactedObjectType ReactedObjectType { get; set; }
    }
}