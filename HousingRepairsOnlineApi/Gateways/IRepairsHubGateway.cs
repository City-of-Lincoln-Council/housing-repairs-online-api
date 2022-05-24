using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Domain.Boundaries;

namespace HousingRepairsOnlineApi.Gateways;

public interface IRepairsHubGateway
{
    Task<(string, bool)> CreateWorkOrder(RepairsHubCreationRequest repairsHubCreationRequest);
}
