using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Smalec.Lib.Social.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReactionType
    {
        [EnumMember(Value = "like")]
        Like,

        [EnumMember(Value = "fuckJu")]
        FuckJu
    }
}