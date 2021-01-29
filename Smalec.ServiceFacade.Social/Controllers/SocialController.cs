using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smalec.Lib.Social.Dtos;
using Smalec.ServiceFacade.Social.Commands;
using Smalec.ServiceFacade.Social.Dtos;
using Smalec.ServiceFacade.Social.Requests;

namespace Smalec.ServiceFacade.Social.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Policy = "ActiveToken")]
    [ApiController]
    public class SocialController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SocialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> ChangeProfilePhoto()
        {
            await _mediator.Publish(new ChangeProfilePhoto
            {
                Photo = HttpContext.Request.Form.Files[0],
                UserUuid = User.Identity.Name
            });

            return Ok();
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> AddPost([FromForm] AddPostDto dto)
        {
            await _mediator.Publish(new AddPost
            {
                Photo = HttpContext.Request.Form.Files.Any() ? HttpContext.Request.Form.Files[0] : null,
                Description = dto.Description
            });

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetMainpagePosts(int page, string lastPostFetched)
        {
            var result = await _mediator.Send(new GetMainpagePosts
            {
                Page = page,
                LastPostFetched = lastPostFetched,
                CurrentUserUuid = User.Identity.Name
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] AddCommentDto data)
        {
            await _mediator.Publish(new AddComment
            {
                Text = data.Text,
                PostOwnerUuid = data.PostOwnerUuid,
                PostUuid = data.PostUuid,
                CurrentUserUuid = User.Identity.Name
            });

            return Ok();
        }
    }
}
