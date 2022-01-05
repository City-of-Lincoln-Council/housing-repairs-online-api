using System;
using System.Net;
using System.Threading.Tasks;
using Azure.Cosmos;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Gateways
{
    public class CosmosGateway : ICosmosGateway
    {
        private static CosmosContainer cosmosContainer;

        public CosmosGateway(CosmosContainer cosmosContainer)
        {
            cosmosContainer = cosmosContainer;
        }

        /// <summary>
        /// Add RepairRequest items to the container
        /// </summary>
        private static async Task AddItemToContainerAsync(RepairRequest repairRequest)
        {
            CosmosContainer container = cosmosContainer;
            try
            {
                // Read the item to see if it exists.
                ItemResponse<RepairRequest> itemResponse = await container.ReadItemAsync<RepairRequest>(repairRequest.Id, new PartitionKey(repairRequest.Id));
                Console.WriteLine("Item in database with id: {0} already exists\n", itemResponse.Value.Id);
            }
            catch (CosmosException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {
                // Create an item in the container. Note we provide the value of the partition key for this item, which is ID
                ItemResponse<RepairRequest> itemResponse = await container.CreateItemAsync<RepairRequest>(repairRequest, new PartitionKey(repairRequest.Id));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse.
                Console.WriteLine("Created item in database with id: {0}\n", itemResponse.Value.Id);
            }
        }
    }

}
