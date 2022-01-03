using System.IO;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Blob
{
    public interface IBlobService
    {
        Task UploadFile(string path, string name, Stream stream);
        Task<Stream> DownloadFile(string path, string name);
        Task DeleteFile(string path, string name);
    }
}
