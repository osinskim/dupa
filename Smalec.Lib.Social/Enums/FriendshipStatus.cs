using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Smalec.Lib.Social.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FriendshipStatus
    {
        [EnumMember(Value = "undefined")]
        UNDEFINED = -1,

        [EnumMember(Value = "pendingSent")]
        PENDING_SENT,

        [EnumMember(Value = "pendingReceived")]
        PENDING_RECEIVED,

        [EnumMember(Value = "acceppted")]
        ACCEPTED
    }
}