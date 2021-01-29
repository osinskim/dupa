using MediatR;

namespace Smalec.Service.Posts.Commands
{
    public class AddPostCommand : INotification
    {
        public string Description { get; set; }
        public string ResourceName { get; set; }
        public string UserUuid { get; set; }
    }
}