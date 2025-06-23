using System.Data;
using Notify.Context;
using Notify.Interfaces;
using Notify.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Notify.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        public readonly NotifyContext _context;
        public FileProcessingService(NotifyContext context)
        {
            _context = context;
        }

        public async Task<FileUploadReturnDTO> ProcessData(CsvUploadDto csvUploadDto)
        {
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM process_csv(:csv_input)";

            command.CommandType = CommandType.Text;

            var param = command.CreateParameter();
            param.ParameterName = "csv_input";
            param.Value = csvUploadDto.CsvContent;
            command.Parameters.Add(param);

            using var reader = await command.ExecuteReaderAsync();

            var errorRows = new List<string>();
            while (await reader.ReadAsync())
            {
                errorRows.Add(reader.GetString(0));
            }
            return new FileUploadReturnDTO{ Inserted = "CSV Processed", Errors = errorRows.ToArray() };
        }
    }
}