using Smalec.Lib.Shared.Helpers;
namespace Smalec.Lib.Shared.Commands
{
    public class UploadSuccessCommand : SynchronizedRequest
    {
        public string Resource { get; set; }
        public string UserUuid { get; set; }
    }
}