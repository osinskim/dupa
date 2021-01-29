using Smalec.Api.Abstraction;
using Dapper;
using System.Threading.Tasks;
using Smalec.Lib.Shared.Helpers;
using System.Data.SqlClient;

namespace Smalec.Api.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        
        public UserRepository(AppSettingsBase appSettings)
        {
            _connectionString = appSettings.ConnectionString;
        }

        public async Task<string> GetUserUuidByEmail(string userName)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return await db.QuerySingleOrDefaultAsync<string>(
                    "SELECT TOP(1) Uuid FROM [Users] WHERE Login = @username",
                    new {username = userName}
                );
            }
        }

        public async Task<string> GetUserPassword(string username)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return await db.QuerySingleOrDefaultAsync<string>(
                "SELECT TOP(1) Password FROM [Users] WHERE Login = @username",
                new {username});
            }
        }
    }
}