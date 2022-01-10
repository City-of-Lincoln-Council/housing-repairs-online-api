using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Controllers;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests
{
    public class RepairRequestsControllerTests : ControllerTests
    {
        private RepairRequestsController sytemUndertest;
        private Mock<ISaveRepairRequestUseCase> saveRepairRequestUseCaseMock;

        public RepairRequestsControllerTests()
        {
            saveRepairRequestUseCaseMock = new Mock<ISaveRepairRequestUseCase>();
            sytemUndertest = new RepairRequestsController(saveRepairRequestUseCaseMock.Object);
        }

        [Fact]
        public async Task TestEndpoint()
        {
            RepairRequest repairRequest = new RepairRequest();
            var result = await sytemUndertest.SaveRepairRequests(repairRequest);

            GetStatusCode(result).Should().Be(200);
            saveRepairRequestUseCaseMock.Verify(x => x.Execute(repairRequest), Times.Once);
        }
    }
}
