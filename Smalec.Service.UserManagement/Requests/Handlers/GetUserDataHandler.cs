using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Service.UserManagement.Abstraction;
using Smalec.Service.UserManagement.DTOs;

namespace Smalec.Service.UserManagement.Requests.Handlers
{
    public class GetUserDataHandler : IRequestHandler<GetUserDataRequest, UserDTO>
    {
        private readonly IUserManagementRepository _repository;

        public GetUserDataHandler(IUserManagementRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDTO> Handle(GetUserDataRequest request, CancellationToken cancellationToken)
            => await _repository.GetUserData(request.UserUuid);
    }
}