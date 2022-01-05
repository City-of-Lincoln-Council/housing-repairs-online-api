using System;
using System.Threading.Tasks;
using Azure.Cosmos;

namespace HousingRepairsOnlineApi.Gateways
{
    public class CosmosGateway : ICosmosGateway
    {
        private static CosmosClient cosmosClient;
        private ICosmosGateway _iCosmosGatewayImplementation;

        public CosmosGateway(string endpointUrl, string authorizationKey, string databaseId, string containerId)
        {
            CosmosClient cosmosClient = new CosmosClient(endpointUrl, authorizationKey);
            CosmosGateway.cosmosClient = cosmosClient;

            CreateDatabaseAsync(databaseId);
            CreateContainerAsync(databaseId, containerId);
        }


        /// <summary>
        /// Create the database if it does not exist
        /// </summary>
        private static async Task CreateDatabaseAsync(string databaseId)
        {
            // Create a new database
            CosmosDatabase database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", database.Id);
        }

        /// <summary>
        /// Create the container if it does not exist.
        /// Specify "/RepairID" as the partition key to ensure good distribution of requests and storage.
        /// </summary>
        /// <returns></returns>
        private static async Task CreateContainerAsync(string databaseId, string containerId )
        {
            // Create a new container
            CosmosContainer container = await cosmosClient.GetDatabase(databaseId).CreateContainerIfNotExistsAsync(containerId, "/RepairID");
            Console.WriteLine("Created Container: {0}\n", container.Id);
        }
    }

}
