using Azure.Storage.Blobs;
using BlobAPI.Interface;

namespace BlobAPI.Services
{
    public class BlobStorageService:IBlobService
    {
        private readonly BlobServiceClient _containerClinet;
        public BlobStorageService(IConfiguration configuration)
        {
            _containerClinet = new BlobServiceClient(configuration["AzureBlobStorage:ConnectionString"]);
        }

        public async Task UploadFile(Stream fileStream,string fileName, string containerName)
        {
            var containerClient = _containerClinet.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
        }

        public async Task<Stream> DownloadFile(string fileName, string containerName)
        {
            var containerClient = _containerClinet.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            
            if (await blobClient.ExistsAsync())
            {
                var downloadInfo = await blobClient.DownloadStreamingAsync();
                return downloadInfo.Value.Content;
            }
            
            return null;
        }
    }
}