using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Smalec.Lib.Shared.ApiUtils.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<MultipartFormDataContent> ConvertFormFileToMultipartForm(this IFormFile formFile)
        {
            var streamContent = new StreamContent(formFile.OpenReadStream());
            var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
                
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            return new MultipartFormDataContent
            {
                { fileContent, "file", formFile.FileName }
            };
        }
    }
}
