using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Controllers;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.ControllersTests
{
    public class AppointmentsControllerTests : ControllerTests
    {
        private AppointmentsController sytemUndertest;
        private Mock<IRetrieveAvailableAppointmentsUseCase> availableAppointmentsUseCaseMock;
        public AppointmentsControllerTests()
        {
            availableAppointmentsUseCaseMock = new Mock<IRetrieveAvailableAppointmentsUseCase>();
            sytemUndertest = new AppointmentsController(availableAppointmentsUseCaseMock.Object);
        }

        [Fact]
        public async Task TestEndpoint()
        {
            const string RepairLocation = "kitchen";
            const string RepairProblem = "cupboards";
            const string RepairIssue = "doorHangingOff";

            const string Uprn = "12345";
            var result = await sytemUndertest.AvailableAppointments(RepairLocation,RepairProblem, RepairIssue, Uprn);
            GetStatusCode(result).Should().Be(200);
            availableAppointmentsUseCaseMock.Verify(x => x.Execute(RepairLocation, RepairProblem, RepairIssue, Uprn), Times.Once);
        }

    }
}
