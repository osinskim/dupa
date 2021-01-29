using MediatR;
using Microsoft.AspNetCore.Http;

namespace Smalec.ServiceFacade.Social.Commands
{
    public class AddPost : INotification
    {
        public IFormFile Photo { get; set; }
        public string Description { get; set; }
    }
}
