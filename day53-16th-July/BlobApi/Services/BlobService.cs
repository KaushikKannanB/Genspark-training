using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using BlobAPI.Interface;

namespace BlobAPI.Services
{
    public class BlobStorageService:IBlobService
    {
        private readonly BlobServiceClient _containerClinet;
        private readonly HttpClient _httpClient;
        public BlobStorageService(IConfiguration configuration)
        {
            string functionUrl = configuration["BlobStorageFunctionUrl"];
            string? connectionString = null;

            _httpClient = new HttpClient();
            var response = _httpClient.GetAsync(functionUrl).Result;

            if (response.IsSuccessStatusCode)
            {
                var raw = response.Content.ReadAsStringAsync().Result;

                // Strip "Fetched Connection String: " prefix
                if (raw.StartsWith("Fetched Connection String: "))
                {
                    connectionString = raw.Replace("Fetched Connection String: ", "").Trim();
                }
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Failed to fetch Blob Storage connection string from Azure Function.");
            }

            _containerClinet = new BlobServiceClient(connectionString);
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