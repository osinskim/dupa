using MediatR;
using Microsoft.AspNetCore.Http;

namespace Smalec.Service.StaticFileStorage.Requests
{
    public class SaveFileRequest : IRequest<string>
    {
        public IFormFile Photo { get; set; }

        public SaveFileRequest(IFormFile photo)
        {
            Photo = photo;
        }
    }
}