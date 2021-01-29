using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Smalec.Service.StaticFileStorage.Requests.Handlers
{
    public class SaveFileHandler : IRequestHandler<SaveFileRequest, string>
    {
        public async Task<string> Handle(SaveFileRequest command, CancellationToken cancellationToken)
        {
            var fileName = Guid.NewGuid().ToString() + GetFileExt(command.Photo);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
                await command.Photo.CopyToAsync(fileStream);

            return fileName;
        }

        private string GetFileExt(IFormFile file)
        {
            return "." + file.FileName.Split(".").Last();
        }
    }
}