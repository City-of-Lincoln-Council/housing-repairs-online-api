using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;
using Microsoft.Azure.Cosmos;

namespace HousingRepairsOnlineApi.Gateways
{
    public class CosmosGateway : IRepairStorageGateway
    {
        private Container cosmosContainer;
        private readonly IIdGenerator idGenerator;

        public CosmosGateway(Container cosmosContainer, IIdGenerator idGenerator)
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

                return itemResponse.Resource;
            }
            catch (CosmosException ex)
            {
                var newRepair = await AddRepair(repair);
                return newRepair;
            }
        }
    }
}
