using System.Collections.Generic;
using Smalec.Service.UserManagement.Abstraction;
using Smalec.Service.UserManagement.Enums;
using Smalec.Lib.Shared.Abstraction.Interfaces;

namespace Smalec.Service.UserManagement.Validators
{
    public class UserExistsValidator : IValidator
    {
        private readonly IUserManagementRepository _userManagementRepository;
        private readonly string _userName;

        public UserExistsValidator(IUserManagementRepository repo, string userName)
        {
            _userManagementRepository = repo;
            _userName = userName;
        }

        public IEnumerable<string> Validate()
        {
            if (!string.IsNullOrEmpty(_userName) && _userManagementRepository.UserExists(_userName).Result) //poprawi�
                yield return ErrorCodes.USER_EXISTS.ToString();
        }
    }
}