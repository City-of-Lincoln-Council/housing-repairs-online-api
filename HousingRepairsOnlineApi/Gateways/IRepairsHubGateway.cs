using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Gateways;

public interface IRepairsHubGateway
{
    Task<bool> CreateWorkOrder(RepairsHubCreationRequest repairsHubCreationRequest);
}