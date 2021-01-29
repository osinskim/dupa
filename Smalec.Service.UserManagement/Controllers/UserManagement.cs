using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Smalec.Service.UserManagement.Commands;
using Smalec.Service.UserManagement.Models;
using Smalec.Service.UserManagement.Requests;
using Microsoft.AspNetCore.Authorization;
using Smalec.Lib.Social.CommandDtos;
using System;

namespace Smalec.Service.UserManagement.Controllers
{
    [ApiController]
    [Route("[action]")]
    [Authorize(Policy="ActiveToken")]
    public class UserManagementController : ControllerBase
    {
        private readonly IMediator _mediator;  
        
        public UserManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUserData()
        {
            return Ok(await _mediator.Send(new GetUserDataRequest(User.Identity.Name)));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserData(string userUuid)
        {
            return Ok(await _mediator.Send(new GetUserDataRequest(userUuid)));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCurrentUserData([FromBody] User user)
        {
            user.Uuid = User.Identity.Name;
            
            await _mediator.Publish(new UpdateCurrentUserDataCommand(user));

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddNewUser([FromBody] CreateUserRequest request)
        {
            var errors = await _mediator.Send(request);
            if(errors.Any())
                return BadRequest(errors);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetProfilePhoto([FromBody] ChangeProfilePhotoCommandDto cmd)
        {
            if (!string.Equals(User.Identity.Name, cmd.UserUuid))
                throw new Exception("no coœ sie popierdoli³o hcyba");

            await _mediator.Publish(new SetProfilePhoto(cmd.UserUuid, cmd.ResourceName));

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keywords)
        {
            return Ok(await _mediator.Send(new SearchRequest(keywords)));
        }
    }
}
