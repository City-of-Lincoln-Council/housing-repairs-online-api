using System.Threading.Tasks;
using Ardalis.GuardClauses;
using HousingRepairsOnlineApi.Domain;
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
    public Task<bool> Execute(RepairRequest repairRequest, Repair result, string token)
    {
        Guard.Against.NullOrWhiteSpace(token, nameof(token));

        var repairsHubCreationRequest = mapRepairsOnlineToRepairsHub.Map(repairRequest, result);
        var createWorkOrderSucceeded = repairsHubGateway.CreateWorkOrder(repairsHubCreationRequest);
        return createWorkOrderSucceeded;
    }
}
