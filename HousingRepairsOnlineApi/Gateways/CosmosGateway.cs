using System;
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
        public async Task<string> AddItemToContainerAsync(Repair repair)
        {

            // Create an item in the container. Note we provide the value of the partition key for this item, which is ID
            ItemResponse<Repair> itemResponse = await cosmosContainer.CreateItemAsync(repair);

            // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse.
            return itemResponse.Value.Id;

        }
    }

}
