using MediatR;

namespace Smalec.Service.UserManagement.Commands
{
    public class SetProfilePhoto: INotification
    {
        public string UserUuid { get; set; }
        public string ResourceName { get; set; }

        public SetProfilePhoto(string userUuid, string resourceName)
        {
            UserUuid = userUuid;
            ResourceName = resourceName;
        }
    }
}