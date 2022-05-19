using System;
using System.Net;
using System.Net.Http;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Gateways;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.GatewaysTests;

public class RepairsHubGatewayTests
{
    private RepairsHubGateway systemUnderTest;
    private readonly MockHttpMessageHandler mockHttp;
    private const string RepairsHubApiEndpoint = "https://repairshub.api";

    public RepairsHubGatewayTests()
    {
        mockHttp = new MockHttpMessageHandler();
        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri(RepairsHubApiEndpoint);

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        systemUnderTest = new RepairsHubGateway(httpClientFactoryMock.Object);
    }

    [Fact]
    public async void GivenNullRepairsHubCreationRequest_WhenCreatingWorkOrder_ThenArgumentNullExceptionIsThrown()
    {
        // Arrange

        //Act

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => systemUnderTest.CreateWorkOrder(null));
    }

    [Fact]
    public async void GivenARepairsHubCreationRequest_WhenCreatingWorkOrder_ThenAWorkOrderRequestIsSent()
    {
        // Arrange
        mockHttp.When($"/api/v2/workOrders/schedule").Respond(HttpStatusCode.OK);

        // Act
        _ = await systemUnderTest.CreateWorkOrder(new RepairsHubCreationRequest());

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }
}
