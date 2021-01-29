using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Service.UserManagement.Abstraction;

namespace Smalec.Service.UserManagement.Commands.Handlers
{
    public class UpdateCurrentUserDataHandler : INotificationHandler<UpdateCurrentUserDataCommand>
    {
        private readonly IUserManagementRepository _repository;
        
        public UpdateCurrentUserDataHandler(IUserManagementRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateCurrentUserDataCommand command, CancellationToken cancellationToken)
            => await _repository.UpdateUserData(command.User);
    }
}