using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Gateways
{
    public interface IRepairStorageGateway
    {
        Task<string> AddRepair(Repair repair);
    }
}
