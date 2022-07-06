using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.Helpers;

namespace HousingRepairsOnlineApi.UseCases
{
    public class SaveRepairRequestUseCase : ISaveRepairRequestUseCase
    {
        private readonly IRepairStorageGateway repairStorageGateway;
        private readonly IBlobStorageGateway storageGateway;
        private readonly ISoREngine sorEngine;

        public SaveRepairRequestUseCase(IRepairStorageGateway repairStorageGateway, IBlobStorageGateway storageGateway, ISoREngine sorEngine)

        {
            this.repairStorageGateway = repairStorageGateway;
            this.storageGateway = storageGateway;
            this.sorEngine = sorEngine;
        }

        public async Task<Repair> Execute(RepairRequest repairRequest, string id)
        {

            var repair = new Repair
            {
                Id = id,
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
                },
                SOR = sorEngine.MapSorCode(
                    repairRequest.Location.Value,
                    repairRequest.Problem.Value,
                    repairRequest.Issue?.Value)
            };

            if (!string.IsNullOrEmpty(repairRequest.Description.Base64Img))
            {
                var photoUrl = storageGateway.UploadBlob(
                    repairRequest.Description.Base64Img,
                    repairRequest.Description.FileExtension
                ).Result;
                repair.Description.PhotoUrl = photoUrl;

            }

            var savedRequest = await repairStorageGateway.AddRepair(repair);

            return savedRequest;
        }
    }
}
