using System;
using System.Linq;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Gateways;

namespace HousingRepairsOnlineApi.UseCases
{
    public class RetrieveImageLinkUseCase : IRetrieveImageLinkUseCase
    {
        private readonly IBlobStorageGateway storageGateway;
        private readonly int daysUntilImageExpiry;

        public RetrieveImageLinkUseCase(IBlobStorageGateway storageGateway, int daysUntilImageExpiry)

        {
            this.storageGateway = storageGateway;
            this.daysUntilImageExpiry = daysUntilImageExpiry;
        }

        public async Task<string> Execute(string photoUrl)
        {
            try
            {
                var fileName = GetFileNameFromPhotoUrl(photoUrl);
                return await storageGateway.GetServiceSasUriForBlob(fileName, daysUntilImageExpiry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string GetFileNameFromPhotoUrl(string photoUrl)
        {
            var photoUrlArray = photoUrl.Split("/");
            var fileName = photoUrlArray.Last();
            return fileName;
        }
    }
}
