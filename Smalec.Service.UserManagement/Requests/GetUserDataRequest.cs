using MediatR;
using Smalec.Service.UserManagement.DTOs;

namespace Smalec.Service.UserManagement.Requests
{
    public class GetUserDataRequest: IRequest<UserDTO>
    {
        public string UserUuid { get; set; }

        public GetUserDataRequest(string userUuid)
        {
            UserUuid = userUuid;
        }
    }
}