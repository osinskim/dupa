using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Smalec.Lib.Social.CommandDtos;
using Smalec.Lib.Shared.Abstraction.Interfaces;
using Smalec.Lib.Shared.ApiUtils.Extensions;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smalec.ServiceFacade.Social.Commands.Handlers
{
    public class ChangeProfilePhotoHandler : INotificationHandler<ChangeProfilePhoto>
    {
        private readonly IConsulHttpClient _http;

        public ChangeProfilePhotoHandler(IConsulHttpClient http)
        {
            _http = http;
        }

        public async Task Handle(ChangeProfilePhoto notification, CancellationToken cancellationToken)
        {
            var resourceName = await UploadPhoto(notification.Photo);
            await SetNewResourceNameForProfilePhoto(resourceName, notification.UserUuid);
        }

        private async Task<string> UploadPhoto(IFormFile photo)
        {
            var formData = await photo.ConvertFormFileToMultipartForm();

            return await _http.Post("filestorage", "/SaveFile", formData);
        }

        private async Task SetNewResourceNameForProfilePhoto(string resourceName, string userUuid)
        {
            var data = new ChangeProfilePhotoCommandDto
            {
                UserUuid = userUuid,
                ResourceName = resourceName
            };

            var requestBody = JsonConvert.SerializeObject(data);
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            await _http.Post("usermanagement", "/SetProfilePhoto", requestContent);
        }
    }
}
