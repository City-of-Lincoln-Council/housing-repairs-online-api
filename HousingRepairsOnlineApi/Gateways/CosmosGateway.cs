using System;
using System.Net;
using System.Threading.Tasks;
using Azure.Cosmos;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Gateways
{
    public class CosmosGateway : ICosmosGateway
    {
        private CosmosContainer cosmosContainer;

        public CosmosGateway(CosmosContainer cosmosContainer)
        {
            this.cosmosContainer = cosmosContainer;
        }

        /// <summary>
        /// Add RepairRequest items to the container
        /// </summary>
        public async Task<string> AddItemToContainerAsync(RepairRequest repairRequest)
        {

            // Create an item in the container. Note we provide the value of the partition key for this item, which is ID
            ItemResponse<RepairRequest> itemResponse = await cosmosContainer.CreateItemAsync<RepairRequest>(repairRequest);

            // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse.
            Console.WriteLine("Created item in database with id: {0}\n", itemResponse.Value.Id);
            return itemResponse.Value.Id;

        }
    }

}
