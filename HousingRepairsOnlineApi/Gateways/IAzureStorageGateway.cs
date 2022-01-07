using System.Threading.Tasks;

namespace HousingRepairsOnlineApi.Gateways
{
    public interface IAzureStorageGateway
    {
        Task<string> UploadBlob(string base64_img);
    }
}
