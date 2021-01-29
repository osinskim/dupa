namespace Smalec.Service.UserManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string ProfilePhoto { get; set; }
    }
}