using System;

namespace Smalec.Lib.Shared.Helpers
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public string CreatedByUser { get; set; }
    }
}