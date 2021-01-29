using System.Collections.Generic;
using MediatR;
using Smalec.Service.UserManagement.DTOs;

namespace Smalec.Service.UserManagement.Requests
{
    public class SearchRequest: IRequest<IEnumerable<UserDTO>>
    {
        public string Keywords { get; set; }

        public SearchRequest(string keywords)
        {
            Keywords = keywords;
        }
    }
}