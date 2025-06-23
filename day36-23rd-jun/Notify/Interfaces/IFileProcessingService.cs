using Notify.Models.DTO;

namespace Notify.Interfaces
{
    public interface IFileProcessingService
    {
        public Task<FileUploadReturnDTO> ProcessData(CsvUploadDto csvUploadDto);
    }
}