using MediatR;

namespace Smalec.ServiceFacade.Social.Commands
{
    public class AddComment : INotification
    {
        public string Text { get; set; }
        public string PostUuid { get; set; }
        public string PostOwnerUuid { get; set; }
        public string CurrentUserUuid { get; set; }
    }
}
