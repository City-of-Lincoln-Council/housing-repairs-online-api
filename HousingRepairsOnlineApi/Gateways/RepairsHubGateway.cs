using System.Net.Http;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using HousingRepairsOnlineApi.Domain.Boundaries;

namespace HousingRepairsOnlineApi.Gateways;

public class RepairsHubGateway : IRepairsHubGateway
{
    private readonly IHttpClientFactory httpClientFactory;

    public RepairsHubGateway(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<(string, bool)> CreateWorkOrder(RepairsHubCreationRequest repairsHubCreationRequest)
    {
        Guard.Against.Null(repairsHubCreationRequest, nameof(repairsHubCreationRequest));

        var httpClient = httpClientFactory.CreateClient(HttpClientNames.RepairsHub);
        var request = new HttpRequestMessage(HttpMethod.Post,
            $"workOrders/schedule");

        var jsonContent = JsonSerializer.Serialize(repairsHubCreationRequest);
        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await httpClient.SendAsync(request);

        var orderResult = await response.Content.ReadFromJsonAsync<CreateOrderResult>();
        var result = (orderResult.Id.ToString(), response.IsSuccessStatusCode);

        return result;
    }
}
