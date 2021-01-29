using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Smalec.Lib.Social.CommandDtos;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.ApiUtils.Extensions;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smalec.ServiceFacade.Social.Commands.Handlers
{
    public class AddPostHandler : INotificationHandler<AddPost>
    {
        private readonly IConsulHttpClient _http;

        public AddPostHandler(IConsulHttpClient http)
        {
            _http = http;
        }

        public async Task Handle(AddPost notification, CancellationToken cancellationToken)
        {
            var postDto = new AddPostCommandDto
            {
                Description = notification.Description,
                ResourceName = notification.Photo != null ? await SaveFile(notification.Photo) : string.Empty
            };

            var requestBody = JsonConvert.SerializeObject(postDto);
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            await _http.Post("posts", "/AddPost", requestContent);
        }

        private async Task<string> SaveFile(IFormFile photo)
        {
            var formData = await photo.ConvertFormFileToMultipartForm();
            return await _http.Post("filestorage", "/SaveFile", formData);
        }
    }
}
