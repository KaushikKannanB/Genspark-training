using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Company.FunctionApp2;

public class Function
{
    private readonly ILogger<Function> _logger;

    public Function(ILogger<Function> logger)
    {
        _logger = logger;
    }

    [Function("FetchStorageConnection")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "get-storage-conn")] HttpRequestData req)
    {
        string keyVaultUri = Environment.GetEnvironmentVariable("KeyVaultUri");
        string secretName = Environment.GetEnvironmentVariable("StorageConnectionSecretName");

        if (string.IsNullOrEmpty(keyVaultUri) || string.IsNullOrEmpty(secretName))
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Missing KeyVaultUri or StorageConnectionSecretName.");
            return errorResponse;
        }

        var secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

        try
        {
            KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName);
            string connString = secret.Value;

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Fetched Connection String: {connString}");
            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync($"Failed to fetch secret: {ex.Message}");
            return errorResponse;
        }
    }
}
