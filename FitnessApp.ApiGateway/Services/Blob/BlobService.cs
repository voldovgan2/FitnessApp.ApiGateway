using Azure.Storage.Blobs;
using FitnessApp.ApiGateway.Configuration;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Blob
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobService(IOptions<BlobSettings> settings)
        {
            _blobServiceClient = new BlobServiceClient(settings.Value.ConnectionString);
        }

        public async Task UploadFile(string path, string name, Stream stream)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(path);
            blobContainerClient.Create();
            var blob = blobContainerClient.GetBlobClient(name);
            await blob.UploadAsync(stream);
        }

        public async Task<Stream> DownloadFile(string path, string name)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(path);
            var blobClient = blobContainerClient.GetBlobClient(name);
            var downloadResult = await blobClient.DownloadContentAsync();
            var result = downloadResult.Value.Content.ToStream();
            return result;
        }

        public async Task DeleteFile(string path, string name)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(path);
            var blobClient = blobContainerClient.GetBlobClient(name);
            await blobClient.DeleteAsync();
        }
    }
}