using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Service.Posts.Abstraction;
using Smalec.Service.Posts.Models;

namespace Smalec.Service.Posts.Commands.Handlers
{
    public class AddCommentCommandHandler : INotificationHandler<AddCommentCommand>
    {
        private readonly ISocialRepository _repo;
        
        public AddCommentCommandHandler(ISocialRepository repo)
        {
            _repo = repo;
        }
        
        public async Task Handle(AddCommentCommand notification, CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                CreatedDate = DateTime.Now,
                UserUuid = notification.UserUuid,
                Text = notification.Text,
                PostUuid = notification.PostUuid,
                Uuid = Guid.NewGuid().ToString()
            };

            await _repo.AddComment(comment);
        }
    }
}