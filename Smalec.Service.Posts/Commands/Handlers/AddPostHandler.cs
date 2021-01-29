using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Service.Posts.Abstraction;
using Smalec.Service.Posts.Models;

namespace Smalec.Service.Posts.Commands.Handlers
{
    public class AddPostHandler : INotificationHandler<AddPostCommand>
    {
        private readonly ISocialRepository _repo;
        
        public AddPostHandler(ISocialRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(AddPostCommand command, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                Uuid = Guid.NewGuid().ToString(),
                Description = command.Description,
                MediaURL = command.ResourceName ?? string.Empty,
                CreatedDate = DateTime.Now,
                UserUuid = command.UserUuid
            };

            await _repo.AddPost(post);
        }
    }
}