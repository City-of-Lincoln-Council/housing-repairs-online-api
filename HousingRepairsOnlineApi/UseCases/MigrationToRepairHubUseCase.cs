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

    public Task<(string, bool)> Execute(RepairRequest repairRequest, string sorCode)
    {
        Guard.Against.Null(repairRequest, nameof(repairRequest));
        Guard.Against.Null(sorCode, nameof(sorCode));

        var repairsHubCreationRequest = mapRepairsOnlineToRepairsHub.Map(repairRequest, sorCode);
        var result = repairsHubGateway.CreateWorkOrder(repairsHubCreationRequest);

        return result;
    }
}
