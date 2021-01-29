using System.Collections.Generic;
using System.Threading.Tasks;
using Smalec.Service.UserManagement.DTOs;
using Smalec.Service.UserManagement.Models;

namespace Smalec.Service.UserManagement.Abstraction
{
    public interface IUserManagementRepository
    {
        Task AddUser(User user);

        Task<bool> UserExists(string username);

        Task UpdateUserData(User data);

        Task<UserDTO> GetUserData(string userUuid);

        Task SetProfilePhoto(string userUuid, string resource);

        Task<IEnumerable<UserDTO>> Search(string keywords);
    }
}