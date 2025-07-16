namespace BlobAPI.Interface
{
    public interface IBlobService
    {
        Task UploadFile(Stream stream, string fileName, string containerName);
    }
}

