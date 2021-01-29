using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Smalec.Service.Posts.Commands;
using Smalec.Service.Posts.Requests;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using Smalec.Lib.Social.Enums;
using Smalec.Lib.Social.CommandDtos;
using Smalec.Lib.Social.Dtos;

namespace Smalec.Service.Posts.Controllers
{
    [ApiController]
    [Route("[action]")]
    [Authorize(Policy="ActiveToken")]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator; 

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] AddPostCommandDto data)
        {
            var cmd = new AddPostCommand
            {
                UserUuid = User.Identity.Name,
                Description = data.Description,
                ResourceName = data.ResourceName
            };

            await _mediator.Publish(cmd);

            return Ok();
        }

        [HttpGet]
        
        public async Task<IActionResult> GetMyPosts(int page, string lastPostFetched)
        {
            var request = new GetPostsRequest
            {
                CurrentUserUuid = User.Identity.Name,
                FriendsUuid = null,
                Destination = PostDestination.MyProfile,
                Page = page,
                LastPostFetched = lastPostFetched
            };
            var posts = await _mediator.Send(request);

            return Ok(posts);
        }

        [HttpGet]
        
        public async Task<IActionResult> GetMyRecentPost()
        {
            var request = new GetPostsRequest
            {
                CurrentUserUuid = User.Identity.Name,
                FriendsUuid = Enumerable.Empty<string>(),
                Destination = PostDestination.MyNewlyAddedPost,
                Page = 0,
                LastPostFetched = DateTime.Now.ToString()
            };
            var post = (await _mediator.Send(request)).FirstOrDefault();

            return Ok(post);
        }

        [HttpPost]
        
        public async Task<IActionResult> GetMainpagePosts([FromBody] GetMainpagePostsDto data)
        {
            var request = new GetPostsRequest
            {
                CurrentUserUuid = User.Identity.Name,
                FriendsUuid = data.MyFriendsUuids,
                Destination = PostDestination.MainPage,
                Page = data.Page,
                LastPostFetched = data.LastPostFetched
            };
            var posts = await _mediator.Send(request);

            return Ok(posts);
        }

        [HttpGet]
        
        public async Task<IActionResult> GetPostsByUserId(string userUuid, int page, string lastPostFetched)
        {
            var request = new GetPostsRequest
            {
                CurrentUserUuid = User.Identity.Name,
                FriendsUuid = new string[] { userUuid },
                Destination = PostDestination.OtherUserProfile,
                Page = page,
                LastPostFetched = lastPostFetched
            };
            var posts = await _mediator.Send(request);

            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> AddReaction([FromBody] AddReactionCommand command)
        {
            command.UserUuid = User.Identity.Name;
            await _mediator.Publish(command);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] AddCommentDto data)
        {
            await _mediator.Publish(new AddCommentCommand
            {
                UserUuid = User.Identity.Name,
                PostUuid = data.PostUuid,
                Text = data.Text
            });

            return Ok();
        }
    }
}
