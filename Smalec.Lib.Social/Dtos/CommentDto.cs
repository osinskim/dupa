using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Smalec.Lib.Social.Dtos
{
    public class CommentDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("postUuid")]
        public string PostUuid { get; set; }

        [JsonProperty("userUuid")]
        public string UserUuid { get; set; }

        [JsonProperty("reactions")]
        public IEnumerable<ReactionDto> Reactions { get; set; }
    }
}