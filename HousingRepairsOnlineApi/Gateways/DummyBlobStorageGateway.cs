using System.Threading.Tasks;

namespace HousingRepairsOnlineApi.Gateways
{
    public class DummyBlobStorageGateway : IBlobStorageGateway
    {
        private const string urlAddress = "http://dummy.blobstorage/";

        public Task<string> UploadBlob(string base64Img, string fileExtension)
        {
            return Task.FromResult($"{urlAddress}/filename.{fileExtension}");
        }

        public string GetUriForBlob(string blobName, int daysUntilExpiry, string storedPolicyName = null)
        {
            return $"{urlAddress}shareableuri/{blobName}";
        }
    }
}
