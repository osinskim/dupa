using System.Collections.Generic;
using MediatR;

namespace Smalec.Service.UserManagement.Requests
{
    public class CreateUserRequest: IRequest<List<string>>
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }
    }
}