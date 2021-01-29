using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Smalec.Lib.Social.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReactedObjectType
    {
        [EnumMember(Value = "comment")]
        Comment,

        [EnumMember(Value = "post")]
        Post
    }
}