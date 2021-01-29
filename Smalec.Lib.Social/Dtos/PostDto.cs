using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Smalec.Lib.Social.Dtos
{
    public class PostDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("userUuid")]
        public string UserUuid { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("mediaURL")]
        public string MediaURL { get; set; }

        [JsonProperty("reactions")]
        public IEnumerable<ReactionDto> Reactions { get; set; }

        [JsonProperty("comments")]
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}