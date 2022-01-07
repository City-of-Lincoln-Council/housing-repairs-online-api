﻿using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace HousingRepairsOnlineApi.Gateways
{
    public class AzureStorageGateway : IAzureStorageGateway
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

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            byte[] bytes = Convert.FromBase64String(base64Img);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                blobClient.Upload(stream);
            }

            return blobClient.Uri.AbsoluteUri;
        }
    }
}
