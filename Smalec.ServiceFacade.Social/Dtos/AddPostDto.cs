using Newtonsoft.Json;

namespace Smalec.ServiceFacade.Social.Dtos
{
    public class AddPostDto
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
