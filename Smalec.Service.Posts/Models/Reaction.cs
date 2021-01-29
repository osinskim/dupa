using Smalec.Lib.Social.Enums;
using Smalec.Lib.Shared.Abstraction.AbstractClasses;

namespace Smalec.Service.Posts.Models
{
    public class Reaction : UserActivity
    {
        public int Id { get; set; }

        public ReactionType Type { get; set; }

        public int PostId { get; set; }

        public string PostUuid { get; set; }

        public int CommentId { get; set; }

        public string CommentUuid { get; set; }
    }
}