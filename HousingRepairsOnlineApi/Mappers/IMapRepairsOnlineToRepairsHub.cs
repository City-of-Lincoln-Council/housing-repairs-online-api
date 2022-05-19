using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Domain.Boundaries;

namespace HousingRepairsOnlineApi.Mappers;

public interface IMapRepairsOnlineToRepairsHub
{
    RepairsHubCreationRequest Map(RepairRequest repairRequest, Repair result);
}
