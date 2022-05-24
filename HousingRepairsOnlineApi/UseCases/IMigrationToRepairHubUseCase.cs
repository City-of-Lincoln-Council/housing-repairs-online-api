using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface IMigrationToRepairHubUseCase
    {
        Task<(string, bool)> Execute(RepairRequest repairRequest, string sorCode);
    }
}
