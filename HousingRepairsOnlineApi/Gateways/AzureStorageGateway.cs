using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace HousingRepairsOnlineApi.Gateways
{
    public class AzureStorageGateway : IAzureStorageGateway
    {
        private BlobContainerClient storageContainerClient;

        public AzureStorageGateway(BlobContainerClient storageContainerClient)
        {
            this.storageContainerClient = storageContainerClient;
        }

        public async Task<string> UploadBlob(string base64_img)
        {
            string fileName = "test.png";

            // Get a reference to a blob
            BlobClient blobClient = storageContainerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            byte[] bytes = Convert.FromBase64String(base64_img);
            using (MemoryStream stream = new MemoryStream(bytes)){
                blobClient.Upload(stream);
            }

            // blobClient.Upload(base64_img);

            // using(var stream = filepond.OpenReadStream()) {
            // }
            var absoluteUrl= blobClient.Uri.AbsoluteUri;
            return absoluteUrl;
        }
    }
}
