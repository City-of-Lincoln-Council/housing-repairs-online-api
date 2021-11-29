using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Controllers;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.ControllersTests
{
    public class AppointmentsControllerTests : ControllerTests
    {
        private AppointmentsController sytemUndertest;

        public AppointmentsControllerTests()
        {
            sytemUndertest = new AppointmentsController();
        }

        [Fact]
        public async Task TestEndpoint()
        {
            const string RepairCode = "N98765";
            const string Uprn = "12345";
            var result = await sytemUndertest.AvailableAppointments(RepairCode, Uprn);

            GetStatusCode(result).Should().Be(200);
        }
    }
}
