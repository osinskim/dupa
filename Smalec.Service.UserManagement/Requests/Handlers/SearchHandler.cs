using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Smalec.Service.UserManagement.Abstraction;
using Smalec.Service.UserManagement.DTOs;

namespace Smalec.Service.UserManagement.Requests.Handlers
{
    public class SearchHandler : IRequestHandler<SearchRequest, IEnumerable<UserDTO>>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public SearchHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }
        
        public async Task<IEnumerable<UserDTO>> Handle(SearchRequest request, CancellationToken cancellationToken)
            => await _userManagementRepository.Search(request.Keywords);
    }
}