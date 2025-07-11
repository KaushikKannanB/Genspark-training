using Inventory.Interfaces;

namespace Inventory.Services
{


    public class LogUploadService : IHostedService
    {
        private readonly IBlobService _blobService;

        public LogUploadService(IBlobService blobService)
        {
            _blobService = blobService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Construct full path to the log file
            var logFileName = $"log{DateTime.UtcNow:yyyyMMdd}.txt";
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", logFileName);

            Console.WriteLine($" Looking for log file at: {logFilePath}");

            if (File.Exists(logFilePath))
            {
                await using var stream = File.OpenRead(logFilePath);
                var blobFileName = $"log-{DateTime.UtcNow:yyyy-MM-dd}.txt";

                Console.WriteLine($" Uploading as blob: {blobFileName}");

                await _blobService.UploadFile(stream, blobFileName, "logsfiles");

                Console.WriteLine(" Upload complete.");
            }
            else
            {
                Console.WriteLine(" Log file not found. Skipping upload.");
            }
        }


        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}