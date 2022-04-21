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
        private readonly Mock<IIdGenerator> mockIdGenerator;

        public DynamoDBGatewayTests()
        {
            mockDynamoDBContext = new Mock<IDynamoDBContext>();
            mockIdGenerator = new Mock<IIdGenerator>();
            dynamoDbGateway = new DynamoDbGateway(mockDynamoDBContext.Object, mockIdGenerator.Object);
        }

        [Fact]
        public async void AnItemIsAdded()
        {
            var repairId = "ABCD1234";
            var dummyRepair = new Repair();

            mockIdGenerator.Setup(_ => _.Generate()).Returns(repairId);

            mockDynamoDBContext.Setup(x => x.SaveAsync(It.IsAny<Repair>(),
                It.IsAny<CancellationToken>())
            ).Returns(Task.CompletedTask);

            await dynamoDbGateway.AddRepair(dummyRepair);

            mockDynamoDBContext
                .Verify(x => x.SaveAsync(It.IsAny<Repair>(), default), Times.Once);
        }

        [Fact]
        public async void AnIdIsGenerated()
        {
            var repairId = "ABCD1234";
            var dummyRepair = new Repair();

            mockIdGenerator.Setup(_ => _.Generate()).Returns(repairId);

            mockDynamoDBContext.Setup(x => x.SaveAsync(It.IsAny<Repair>(),
                It.IsAny<CancellationToken>())
            ).Returns(Task.CompletedTask);

            await dynamoDbGateway.AddRepair(dummyRepair);

            mockIdGenerator
                .Verify(x => x.Generate(), Times.Once);
        }
    }
}
