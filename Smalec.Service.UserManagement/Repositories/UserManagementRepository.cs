using System.Threading.Tasks;
using Smalec.Lib.Shared.Helpers;
using Smalec.Service.UserManagement.Abstraction;
using Dapper;
using System.Data.SqlClient;
using Smalec.Service.UserManagement.DTOs;
using System.Collections.Generic;
using Smalec.Service.UserManagement.Models;

namespace Smalec.Service.UserManagement.Repositories
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly string _connectionString;

        public UserManagementRepository(AppSettingsBase appSettings)
        {
            _connectionString = appSettings.ConnectionString;
        }

        public async Task AddUser(User user)
        {
            using (var db = new SqlConnection(_connectionString))
            await db.QueryAsync(
                "INSERT INTO [Users] (Login, Password, Name, Uuid) VALUES (@login, @password, @name, @uuid)",
                new {
                    login = user.Login,
                    password = user.Password,
                    name = user.Name,
                    uuid = user.Uuid
                }
            );
        }

        public async Task<bool> UserExists(string username)
        {
            using (var db = new SqlConnection(_connectionString))
            return await db.QuerySingleOrDefaultAsync<bool>(
                "SELECT 1 FROM [Users] WHERE Login = @username",
                new {username}
            );
        }

        public async Task<UserDTO> GetUserData(string userUuid)
        {
            using (var db = new SqlConnection(_connectionString))
            return await db.QuerySingleOrDefaultAsync<UserDTO>(
                "SELECT TOP(1) Uuid, Name, ProfilePhoto FROM [Users] WHERE Uuid = @userUuid",
                new { userUuid }
            );
        }

        public async Task UpdateUserData(User data)
        {
            var sql = @"
            UPDATE [Users] 
            SET Name = @name
            WHERE Uuid = @uuid";

            using (var db = new SqlConnection(_connectionString))
            await db.QueryAsync(sql, new {name = data.Name, uuid = data.Uuid});
        }

        public async Task SetProfilePhoto(string userUuid, string resource)
        {
            var sql = $@"
            UPDATE [Users]
            SET ProfilePhoto = @{nameof(resource)}
            WHERE Uuid = @{nameof(userUuid)}";

            using (var db = new SqlConnection(_connectionString))
            await db.QueryAsync(sql, new { resource, userUuid });
        }

        public async Task<IEnumerable<UserDTO>> Search(string keywords)
        {
            using (var db = new SqlConnection(_connectionString))
            return await db.QueryAsync<UserDTO>(
                $"SELECT Uuid, Name, ProfilePhoto FROM [Users] WHERE Name LIKE @{nameof(keywords)}",
                new { keywords = AddWildcard(keywords) }
            );
        }

        private string AddWildcard(string keywords)
        {
            return "%" + keywords + "%";
        }
    }
}