using System.Threading;
using Azure.Cosmos;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Gateways;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.GatewaysTests
{
    public class CosmosGatewayTests
    {
        private readonly CosmosGateway azureStorageGateway;
        private readonly Mock<CosmosContainer> mockCosmosContainer;

        public CosmosGatewayTests()
        {
            mockCosmosContainer = new Mock<CosmosContainer>();

            azureStorageGateway = new CosmosGateway(mockCosmosContainer.Object);
        }

        [Fact]
        public async void AnItemIsAdded()
        {
            var repairId = "ABCD1234";
            var dummyRepair = new Repair()
            {
                Id = repairId
            };

            var responseMock = new Mock<ItemResponse<Repair>>();
            responseMock.Setup(_ => _.Value).Returns(dummyRepair);

            // Arrange
            mockCosmosContainer.Setup(_ => _.CreateItemAsync(
               dummyRepair,
               null,
               null,
               default(CancellationToken)
               )).ReturnsAsync(responseMock.Object);

            var actual = await azureStorageGateway.AddItemToContainerAsync(dummyRepair);

            // Assert
            Assert.Equal(repairId, actual);
        }
    }
}
