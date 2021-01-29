using Smalec.Lib.Shared.Helpers;

namespace Smalec.Service.Friendship.Utils
{
    public class AppSettings: AppSettingsBase
    {
        public string NeoServer { get; set; }
        public string NeoUsername { get; set; }
        public string NeoPassword { get; set; }
    }
}
