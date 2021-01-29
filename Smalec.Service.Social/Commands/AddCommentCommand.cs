using MediatR;
using Newtonsoft.Json;

namespace Smalec.Service.Social.Commands
{
    public class AddCommentCommand : INotification
    {
        [JsonIgnore]
        public string UserId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }
    }
}