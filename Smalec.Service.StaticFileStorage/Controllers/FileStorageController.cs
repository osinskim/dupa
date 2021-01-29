using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smalec.Service.StaticFileStorage.Requests;

namespace Smalec.Service.StaticFileStorage.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class FileStorageController : ControllerBase
    {
        private readonly IMediator _mediator;  
        
        public FileStorageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy="ActiveToken")]
        public async Task<IActionResult> GetFile(string resource)
        {
            if(string.IsNullOrEmpty(resource))
                return NotFound();

            //zmieniæ to ¿eby by³o asynchroniczne
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), resource);
            
            if (System.IO.File.Exists(filePath))
            {
                var image = System.IO.File.OpenRead(filePath);
                return File(image, "image/jpeg");
            }
            
            return NotFound();
        }

        [HttpPost, DisableRequestSizeLimit]
        [Authorize(Policy = "ActiveToken")]
        public async Task<IActionResult> SaveFile()
        {
            if (HttpContext.Request.Form.Files.Count == 0)
                return BadRequest();

            var resourceName = await _mediator.Send(new SaveFileRequest(HttpContext.Request.Form.Files[0]));

            return Ok(resourceName);
        }
    }
}