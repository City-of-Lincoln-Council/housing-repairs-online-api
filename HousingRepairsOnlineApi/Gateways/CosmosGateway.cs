using System;
using System.Threading.Tasks;
using Azure.Cosmos;

namespace HousingRepairsOnlineApi.Gateways
{
    public class CosmosGateway : ICosmosGateway
    {
        private CosmosContainer cosmosContainer;

        public CosmosGateway(CosmosContainer cosmosContainer)
        {
            cosmosContainer = cosmosContainer;
        }

    }

}
