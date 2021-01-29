using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Service.UserManagement.Abstraction;

namespace Smalec.Service.UserManagement.Commands.Handlers
{
    public class SetProfilePhotoHandler : INotificationHandler<SetProfilePhoto>
    {
        private readonly IUserManagementRepository _repository;

        public SetProfilePhotoHandler(IUserManagementRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(SetProfilePhoto notification, CancellationToken cancellationToken)
            => await _repository.SetProfilePhoto(notification.UserUuid, notification.ResourceName);
    }
}