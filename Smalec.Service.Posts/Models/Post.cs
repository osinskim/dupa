using System.Collections.Generic;
using Smalec.Lib.Shared.Abstraction.AbstractClasses;

namespace Smalec.Service.Posts.Models
{
    public class Post : UserActivity
    {
        public int Id { get; set; }

        public string Uuid { get; set; }

        public string Description { get; set; }

        public string MediaURL { get; set; }

        public IEnumerable<Reaction> Reactions { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}