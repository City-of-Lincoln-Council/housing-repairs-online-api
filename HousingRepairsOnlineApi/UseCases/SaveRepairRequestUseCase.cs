using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.Helpers;

namespace HousingRepairsOnlineApi.UseCases
{
    public class SaveRepairRequestUseCase : ISaveRepairRequestUseCase
    {
        private readonly ICosmosGateway cosmosGateway;
        private readonly IAzureStorageGateway storageGateway;
        private readonly ISoREngine sorEngine;

        public SaveRepairRequestUseCase(ICosmosGateway cosmosGateway, IAzureStorageGateway storageGateway, ISoREngine sorEngine)

        {
            this.cosmosGateway = cosmosGateway;
            this.storageGateway = storageGateway;
            this.sorEngine = sorEngine;
        }

        public async Task<string> Execute(RepairRequest repairRequest)
        {
            var photoUrl = storageGateway.UploadBlob(repairRequest.Description.Base64Img, repairRequest.Description.FileExtension).Result;

            var repair = new Repair
            {
                Id = Guid.NewGuid().ToString().GetHashCode().ToString("x").ToUpper(),
                Address = repairRequest.Address,
                Postcode = repairRequest.Postcode,
                Location = repairRequest.Location,
                ContactDetails = repairRequest.ContactDetails,
                Problem = repairRequest.Problem,
                Issue = repairRequest.Issue,
                ContactPersonNumber = repairRequest.ContactPersonNumber,
                Time = repairRequest.Time,
                Description = new RepairDescription
                {
                    Text = repairRequest.Description.Text,
                    PhotoUrl = photoUrl
                },
                SOR = sorEngine.MapSorCode(repairRequest.Location.Value, repairRequest.Problem.Value, repairRequest.Issue.Value)
            };
            // TODO: TEST NON UNIQUE ID
            var savedRequest = await cosmosGateway.AddItemToContainerAsync(repair);

            return savedRequest;
        }
    }
}
