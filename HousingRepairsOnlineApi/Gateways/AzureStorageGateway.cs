using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace HousingRepairsOnlineApi.Gateways
{
    public class AzureStorageGateway : IBlobStorageGateway
    {
        private BlobContainerClient storageContainerClient;

        public AzureStorageGateway(BlobContainerClient storageContainerClient)
        {
            this.storageContainerClient = storageContainerClient;
        }

        public async Task<string> UploadBlob(string base64Img, string fileExtension)
        {
            string fileName = $"{Guid.NewGuid().ToString()}.{fileExtension}";

            BlobClient blobClient = storageContainerClient.GetBlobClient(fileName);

            byte[] bytes = Convert.FromBase64String(base64Img);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                blobClient.Upload(stream);
            }

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<string> GetServiceSasUriForBlob(string blobName, int daysUntilExpiry, string storedPolicyName = null)
        {

            var blobClient = storageContainerClient.GetBlobClient(blobName);

            if (blobClient.CanGenerateSasUri)
            {
                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = storageContainerClient.Name,
                    BlobName = blobClient.Name,
                    Resource = "b"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddDays(daysUntilExpiry);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                var sasUri = blobClient.GenerateSasUri(sasBuilder);
                return sasUri.AbsoluteUri;
            }
            else
            {
                throw new Exception(
                    "BlobClient must be authorized with Shared Keycredentials to create a service SAS.");
            }
        }
    }
}
