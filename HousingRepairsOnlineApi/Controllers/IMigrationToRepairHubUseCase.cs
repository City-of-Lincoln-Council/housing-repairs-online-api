using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Controllers
{
    public interface IMigrationToRepairHubUseCase
    {
        Task<bool> Execute(RepairRequest repairRequest, Repair result, string token);
    }
}
