using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Gateways
{
    public interface ICosmosGateway
    {
        Task<string> AddItemToContainerAsync(Repair repair);
    }
}
