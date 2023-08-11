using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;

namespace Company.Function
{
    public static class GetResumeCounter
    {
        private static CosmosClient _cosmosClient;
        private static Container _counterContainer;

        // Entry point of the Azure Function
        [Function("GetResumeCounter")]
        public static async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext context)
        {
            // Initialize logger
            var logger = context.GetLogger("GetResumeCounter");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                // Ensure Cosmos DB is initialized
                EnsureCosmosDbInitialized(context);

                // Define partition key and read counter from Cosmos DB
                var partitionKey = new PartitionKey("1");
                var counterResponse = await _counterContainer.ReadItemAsync<Counter>("1", partitionKey);
                var counter = counterResponse.Resource;

                if (counter == null)
                {
                    logger.LogInformation("Counter object retrieved from Cosmos DB is null.");
                    // Create a new counter object if not found
                    counter = new Counter
                    {
                        id = "1", // Set the counter ID
                        Count = 0
                    };
                }
                else
                {
                    logger.LogInformation($"Counter object retrieved from Cosmos DB: {JsonConvert.SerializeObject(counter)}");
                }

                // Increment the counter
                counter.Count++;

                // Save the updated counter back to Cosmos DB
                var response = await _counterContainer.UpsertItemAsync(counter);

                // Convert response to JSON
                var jsonToReturn = JsonConvert.SerializeObject(response.Resource);

                // Create response message
                var responseMessage = req.CreateResponse(System.Net.HttpStatusCode.OK);
                responseMessage.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await responseMessage.WriteStringAsync(jsonToReturn, Encoding.UTF8);

                return responseMessage;
            }
            catch (Exception ex)
            {
                // Handle errors and create error response
                logger.LogError($"An error occurred: {ex.Message}");
                logger.LogError(ex.StackTrace); // Log the stack trace

                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                errorResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                await errorResponse.WriteStringAsync("An error occurred while processing your request.");
                return errorResponse;
            }
        }

        // Initialize Cosmos DB client and container
        private static void EnsureCosmosDbInitialized(FunctionContext context)
        {
            if (_cosmosClient == null)
            {
                // Get Cosmos DB connection string from environment variable
                var cosmosDbConnectionString = Environment.GetEnvironmentVariable("AzureResumeConnectionString");
                _cosmosClient = new CosmosClient(cosmosDbConnectionString);

                // Replace with your actual database ID and container ID
                var databaseId = "azure-resume";
                var containerId = "counter";
                _counterContainer = _cosmosClient.GetContainer(databaseId, containerId);
            }
        }
    }
}
