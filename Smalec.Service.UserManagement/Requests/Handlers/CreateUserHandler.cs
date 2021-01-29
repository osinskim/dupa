using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Service.UserManagement.Abstraction;
using Smalec.Service.UserManagement.Models;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.Helpers;

namespace Smalec.Service.UserManagement.Requests.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, List<string>>
    {
        private readonly IUserManagementRepository _userManagementRepository;
        private readonly IValidatorsFactory _validatorsFactory;

        public CreateUserHandler(IUserManagementRepository userManagementRepository, IValidatorsFactory validatorsFactory)
        {
            _userManagementRepository = userManagementRepository;
            _validatorsFactory = validatorsFactory;
        }
        
        public async Task<List<string>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Login = request.UserName,
                Password = request.Password,
                Name = request.Name
            };
            var validators = _validatorsFactory.GetValidator(user);
            var validationResult = validators.Validate().ToList();

            if (validationResult.Count != 0)
                return validationResult;

            user.Password = PasswordHelper.GenerateHash(request.Password);
            user.Uuid = Guid.NewGuid().ToString();

            await _userManagementRepository.AddUser(user);

            return new List<string>();
        }
    }
}