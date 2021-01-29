namespace Smalec.Lib.Shared.Helpers
{
    public class AppSettingsBase
    {
        public string ConnectionString { get; set; }

        public string Secret { get; set; }

        public string RedisAddress { get; set; }
    }
}