namespace Inventory.Interfaces
{
    public interface IBlobService
    {
        Task UploadFile(Stream stream, string fileName, string containerName);
        Task<Stream> DownloadFile(string filename, string containername);
    }
}
