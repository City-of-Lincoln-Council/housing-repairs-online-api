using System;
using System.Threading.Tasks;
using Azure.Cosmos;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;

namespace HousingRepairsOnlineApi.Gateways
{
    public class CosmosGateway : IRepairStorageGateway
    {
        private CosmosContainer cosmosContainer;
        private readonly IIdGenerator idGenerator;

        public CosmosGateway(CosmosContainer cosmosContainer, IIdGenerator idGenerator)
        {
            this.cosmosContainer = cosmosContainer;
            this.idGenerator = idGenerator;
        }

        /// <summary>
        /// Add Repair items to the container
        /// </summary>
        public async Task<Repair> AddRepair(Repair repair)
        {
            repair.Id = idGenerator.Generate();

            try
            {
                ItemResponse<Repair> itemResponse = await cosmosContainer.CreateItemAsync(repair);

                return itemResponse.Value;
            }
            catch (CosmosException ex)
            {
                var newRepair = await AddRepair(repair);
                return newRepair;
            }
        }
    }
}
