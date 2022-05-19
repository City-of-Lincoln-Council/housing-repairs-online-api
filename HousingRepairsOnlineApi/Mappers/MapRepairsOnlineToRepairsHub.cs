using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Domain.Boundaries;

namespace HousingRepairsOnlineApi.Mappers;

public class MapRepairsOnlineToRepairsHub : IMapRepairsOnlineToRepairsHub
{
    public RepairsHubCreationRequest Map(RepairRequest repairRequest, Repair result)
    {
        return new RepairsHubCreationRequest
        {

        };
    }
}
