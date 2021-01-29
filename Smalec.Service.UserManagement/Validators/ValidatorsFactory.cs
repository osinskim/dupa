using System;
using System.Collections.Generic;
using System.Linq;
using Smalec.Service.UserManagement.Abstraction;
using Smalec.Service.UserManagement.Models;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.Helpers;

namespace Smalec.Service.UserManagement.Validators
{
    public class ValidatorsFactory : IValidatorsFactory
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public ValidatorsFactory(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }
        
        public Validator GetValidator<T>(T model) where T : class
        {
            switch(typeof(T))
            {
                case Type t when t == typeof(User):
                {
                    return new Validator(
                        new List<IValidator> {
                            new UserExistsValidator(_userManagementRepository, (model as User)?.Login ?? string.Empty),
                            new PasswordValidator((model as User)?.Password ?? string.Empty),
                            new EmailValidator((model as User)?.Login ?? string.Empty)
                        }
                    );
                }
                default:
                    return new Validator(Enumerable.Empty<IValidator>());
            }
        }
    }
}