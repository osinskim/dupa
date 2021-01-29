using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Smalec.Lib.Social.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PostDestination
    {
        [EnumMember(Value = "mainPage")]
        MainPage,

        [EnumMember(Value = "otherUserProfile")]
        OtherUserProfile,

        [EnumMember(Value = "myProfile")]
        MyProfile,

        [EnumMember(Value = "myNewlyAddedPost")]
        MyNewlyAddedPost
    }
}