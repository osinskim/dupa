using MediatR;
using Smalec.Service.UserManagement.Models;

namespace Smalec.Service.UserManagement.Commands
{
    public class UpdateCurrentUserDataCommand: INotification
    {
        public User User { get; set; }

        public UpdateCurrentUserDataCommand(User user)
        {
            User = user;
        }
    }
}