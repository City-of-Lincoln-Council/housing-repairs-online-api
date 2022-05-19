using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Mappers;

public interface IMapRepairsOnlineToRepairsHub
{
    RepairsHubCreationRequest Map(RepairRequest repairRequest, Repair result);
}