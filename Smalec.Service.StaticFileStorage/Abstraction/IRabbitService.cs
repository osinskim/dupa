using System.Threading.Tasks;

namespace Smalec.Service.StaticFileStorage.Abstraction
{
    public interface IRabbitService
    {
        Task SendPhotoUploadSuccess(string fileName, string requestId, string userId);
    }
}