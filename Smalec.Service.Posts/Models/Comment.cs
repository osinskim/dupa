using System.Collections.Generic;
using Smalec.Lib.Shared.Abstraction.AbstractClasses;

namespace Smalec.Service.Posts.Models
{
    public class Comment : UserActivity
    {
        public int Id { get; set; }

        public string Uuid { get; set; }

        public string Text { get; set; }

        public IEnumerable<Reaction> Reactions { get; set; }

        public int PostId { get; set; }
        
        public string PostUuid { get; set; }
    }
}