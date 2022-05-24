using System.Threading.Tasks;
using Ardalis.GuardClauses;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Domain.Boundaries;
using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.Mappers;

namespace HousingRepairsOnlineApi.UseCases;

public class MigrationToRepairHubUseCase : IMigrationToRepairHubUseCase
{
    private readonly IRepairsHubGateway repairsHubGateway;
    private readonly IMapRepairsOnlineToRepairsHub mapRepairsOnlineToRepairsHub;

    public MigrationToRepairHubUseCase(IRepairsHubGateway repairsHubGateway,
        IMapRepairsOnlineToRepairsHub mapRepairsOnlineToRepairsHub)
    {
        this.repairsHubGateway = repairsHubGateway;
        this.mapRepairsOnlineToRepairsHub = mapRepairsOnlineToRepairsHub;
    }

    public Task<CreateWorkOrderResponse> Execute(RepairRequest repairRequest)
    {
        Guard.Against.Null(repairRequest, nameof(repairRequest));

        var repairsHubCreationRequest = mapRepairsOnlineToRepairsHub.Map(repairRequest);
        var result = repairsHubGateway.CreateWorkOrder(repairsHubCreationRequest);

        return result;
    }
}
