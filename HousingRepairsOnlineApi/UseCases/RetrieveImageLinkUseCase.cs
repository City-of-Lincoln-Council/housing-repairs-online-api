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

        public string Execute(string photoUrl)
        {
            try
            {
                var blobName = GetBlobNameFromPhotoUrl(photoUrl);
                return storageGateway.GetUriForBlob(blobName, daysUntilImageExpiry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string GetBlobNameFromPhotoUrl(string photoUrl)
        {
            var photoUrlArray = photoUrl.Split("/");
            var fileName = photoUrlArray.Last();
            return fileName;
        }
    }
}
