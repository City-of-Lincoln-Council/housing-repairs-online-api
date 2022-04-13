using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Ardalis.GuardClauses;

namespace HousingRepairsOnlineApi.Gateways
{
    public class AwsS3BlobStorageGateway : IBlobStorageGateway
    {
        private IAmazonS3 s3Client;
        private readonly ITransferUtility transferUtility;
        private readonly string bucketName;
        private readonly int daysUntilImageExpiry;

        public AwsS3BlobStorageGateway(IAmazonS3 s3Client, ITransferUtility transferUtility,
            string bucketName, int daysUntilImageExpiry)
        {
            this.s3Client = s3Client;
            this.transferUtility = transferUtility;
            this.bucketName = bucketName;
            this.daysUntilImageExpiry = daysUntilImageExpiry;
        }

        public Task<string> UploadBlob(string base64Img, string fileExtension)
        {
            Guard.Against.NullOrWhiteSpace(base64Img, nameof(base64Img));
            Guard.Against.NullOrWhiteSpace(fileExtension, nameof(fileExtension));

            var bytes = Convert.FromBase64String(base64Img);
            var contents = new MemoryStream(bytes);

            var key = $"{Guid.NewGuid().ToString()}.{fileExtension}";
            var uploadTask = transferUtility.UploadAsync(contents, bucketName, key);

            var result = uploadTask.ContinueWith(t =>
                s3Client.GeneratePreSignedURL(bucketName, key, DateTime.UtcNow.AddDays(daysUntilImageExpiry), null)
            );

            return result;
        }

        public string GetUriForBlob(string blobName, int daysUntilExpiry, string storedPolicyName = null)
        {
            throw new NotImplementedException();
        }
    }
}
