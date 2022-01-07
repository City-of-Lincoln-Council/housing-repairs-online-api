using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Gateways;

namespace HousingRepairsOnlineApi.UseCases
{
    public class SaveRepairRequestUseCase : ISaveRepairRequestUseCase
    {
        private readonly ICosmosGateway cosmosGateway;
        private readonly IAzureStorageGateway storageGateway;

        public SaveRepairRequestUseCase(ICosmosGateway cosmosGateway, IAzureStorageGateway storageGateway)

        {
            this.cosmosGateway = cosmosGateway;
            this.storageGateway = storageGateway;
        }

        public async Task<string> Execute(RepairRequest repairRequest)
        {
            // TODO: TEST NON UNIQUE ID
            repairRequest.Id = Guid.NewGuid().ToString().GetHashCode().ToString("x").ToUpper();
            repairRequest.Description.photo_url = storageGateway.UploadBlob(repairRequest.Description.base64_img).Result;
            var savedRequest = await cosmosGateway.AddItemToContainerAsync(repairRequest);

            return savedRequest;
        }
    }
}
