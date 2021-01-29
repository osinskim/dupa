using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Smalec.Lib.Social.Dtos;
using Smalec.Service.Posts.Abstraction;

namespace Smalec.Service.Posts.Requests.Handlers
{
    public class GetPostsHandler : IRequestHandler<GetPostsRequest, IEnumerable<PostDto>>
    {
        private readonly IEnumerable<IGetPostsStrategy> _strategies;
        private readonly IPostsBuilder _builder;

        public GetPostsHandler(IServiceProvider serviceProvider, IPostsBuilder builder)
        {
            _builder = builder;
            _strategies = serviceProvider.GetServices(typeof(IGetPostsStrategy)) as IEnumerable<IGetPostsStrategy>;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetPostsRequest request, CancellationToken cancellationToken)
        {
            if(request.Page <= 0)
            {
                request.Page = 1;
            }

            var getPostsStrategy = _strategies
                .Where(x => x.CanGet(request))
                .FirstOrDefault();
            
            if(getPostsStrategy == null)
            {
                return Enumerable.Empty<PostDto>();
            }

            var rawData = await getPostsStrategy.GetRawPostsData(request);
            var result = _builder.Build(rawData);

            return result;
        }
    }
}