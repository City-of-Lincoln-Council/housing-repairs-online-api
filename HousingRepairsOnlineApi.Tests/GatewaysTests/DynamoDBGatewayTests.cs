using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.Helpers;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.GatewaysTests
{
    public class DynamoDBGatewayTests
    {
        private readonly DynamoDbGateway dynamoDbGateway;
        private readonly Mock<IDynamoDBContext> mockDynamoDBContext;

        public DynamoDBGatewayTests()
        {
            mockDynamoDBContext = new Mock<IDynamoDBContext>();
            dynamoDbGateway = new DynamoDbGateway(mockDynamoDBContext.Object);
        }

        [Fact]
        public async void AnItemIsAdded()
        {
            var repairId = "ABCD1234";
            var dummyRepair = new Repair();

            mockDynamoDBContext.Setup(x => x.SaveAsync(It.IsAny<Repair>(),
                It.IsAny<CancellationToken>())
            ).Returns(Task.CompletedTask);

            await dynamoDbGateway.AddRepair(dummyRepair);

            mockDynamoDBContext
                .Verify(x => x.SaveAsync(It.IsAny<Repair>(), default), Times.Once);
        }
    }
}
