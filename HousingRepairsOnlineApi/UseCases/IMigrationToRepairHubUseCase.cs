using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Domain.Boundaries;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface IMigrationToRepairHubUseCase
    {
        Task<CreateWorkOrderResponse> Execute(RepairRequest repairRequest);
    }
}
