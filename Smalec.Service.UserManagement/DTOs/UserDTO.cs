using Newtonsoft.Json;

namespace Smalec.Service.UserManagement.DTOs
{
    public class UserDTO
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("profilePhoto")]
        public string ProfilePhoto { get; set; }
    }
}