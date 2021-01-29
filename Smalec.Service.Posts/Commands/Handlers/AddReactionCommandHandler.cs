using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Lib.Social.Enums;
using Smalec.Service.Posts.Abstraction;
using Smalec.Service.Posts.Models;

namespace Smalec.Service.Posts.Commands.Handlers
{
    public class AddReactionCommandHandler : INotificationHandler<AddReactionCommand>
    {
        private readonly ISocialRepository _repo;
        
        public AddReactionCommandHandler(ISocialRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(AddReactionCommand notification, CancellationToken cancellationToken)
        {
 
            var reaction = new Reaction
            {
                Type = notification.Reaction,
                PostUuid = notification.ReactedObjectType == ReactedObjectType.Post ? notification.ObjectUuid : null,
                CommentUuid = notification.ReactedObjectType == ReactedObjectType.Comment ? notification.ObjectUuid : null,
                CreatedDate = DateTime.Now,
                UserUuid = notification.UserUuid
            };

            await _repo.AddOrUpdateReaction(reaction);
        }
    }
}