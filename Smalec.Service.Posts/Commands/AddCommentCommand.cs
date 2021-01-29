using MediatR;

namespace Smalec.Service.Posts.Commands
{
    public class AddCommentCommand : INotification
    {
        public string UserUuid { get; set; }

        public string Text { get; set; }

        public string PostUuid { get; set; }
    }
}