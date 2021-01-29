using MediatR;
using Microsoft.AspNetCore.Http;

namespace Smalec.ServiceFacade.Social.Commands
{
    public class ChangeProfilePhoto : INotification
    {
        public IFormFile Photo { get; set; }

        public string UserUuid { get; set; }
    }
}
