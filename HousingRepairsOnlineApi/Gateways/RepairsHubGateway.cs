using System.Net.Http;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Gateways;

public class RepairsHubGateway : IRepairsHubGateway
{
    private readonly IHttpClientFactory httpClientFactory;

    public RepairsHubGateway(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<bool> CreateWorkOrder(RepairsHubCreationRequest repairsHubCreationRequest)
    {
        var httpClient = httpClientFactory.CreateClient(HttpClientNames.RepairsHub);
        var request = new HttpRequestMessage(HttpMethod.Post,
            $"/api/v2/workOrders/schedule");
        var response = await httpClient.SendAsync(request);


        return await Task.FromResult<bool>(true);
    }
}
