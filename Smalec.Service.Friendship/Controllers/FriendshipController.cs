using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smalec.Service.Friendship.Commands;
using Smalec.Service.Friendship.Requests;
using System.Threading.Tasks;

namespace Smalec.Service.Friendship.Controllers
{
    [Route("[action]")]
    [ApiController]
    [Authorize(Policy = "ActiveToken")]
    public class FriendshipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FriendshipController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddFriendCommand cmd)
        {
            cmd.CurrentUser = User.Identity.Name;
            await _mediator.Publish(cmd);

            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Accept([FromBody] AcceptFriendshipCommand cmd)
        {
            // zrobić tak żeby obecnie zalogowany użytkownik nie mógł zaakceptować sam jak sam wysłał
            cmd.CurrentUser = User.Identity.Name;
            await _mediator.Publish(cmd);

            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Reject([FromBody] RejectFriendshipCommand cmd)
        {
            cmd.CurrentUser = User.Identity.Name;
            await _mediator.Publish(cmd);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> CheckStatus(string userToCheck)
        {
            var result = await _mediator.Send(new CheckFriendshipStatusRequest(User.Identity.Name, userToCheck));

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFriendsUuids(string currentUserUuid) // zmienić to żeby brało uuid z claim
        {
            var result = await _mediator.Send(new GetUserFriendsUuids
            {
                CurrentUserUuid = currentUserUuid
            });

            return Ok(result);
        }
    }
}
