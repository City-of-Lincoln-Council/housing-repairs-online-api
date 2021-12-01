using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.UseCasesTests
{
    public class RetrieveAvailableAppointmentsUseCaseTests
    {
        private readonly RetrieveAvailableAppointmentsUseCase sytemUndertest;
        private readonly Mock<IAppointmentsGateway> appointmentsGatewayMock;
        private readonly Mock<ISoREngine> sorEngineMock;
        const string kitchen = "kitchen";
        const string cupboards = "cupboards";
        const string doorHangingOff = "doorHangingOff";

        public RetrieveAvailableAppointmentsUseCaseTests()
        {
            sorEngineMock = new Mock<ISoREngine>();
            appointmentsGatewayMock = new Mock<IAppointmentsGateway>();
            sytemUndertest = new RetrieveAvailableAppointmentsUseCase(appointmentsGatewayMock.Object, sorEngineMock.Object);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentTestData))]
#pragma warning disable xUnit1026
        public async void GivenAnInvalidRepairLocation_WhenExecute_ThenExceptionIsThrown<T>(T exception, string repairLocation) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange
            var systemUnderTest = new RetrieveAvailableAppointmentsUseCase(appointmentsGatewayMock.Object, sorEngineMock.Object);

            // Act
            Func<Task> act = async () => await systemUnderTest.Execute(repairLocation, cupboards, doorHangingOff, "A UPRN");

            // Assert
            await act.Should().ThrowExactlyAsync<T>();
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentTestData))]
#pragma warning disable xUnit1026
        public async void GivenAnInvalidRepairProblem_WhenExecute_ThenExceptionIsThrown<T>(T exception, string repairProblem) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange
            var systemUnderTest = new RetrieveAvailableAppointmentsUseCase(appointmentsGatewayMock.Object, sorEngineMock.Object);

            // Act
            Func<Task> act = async () => await systemUnderTest.Execute(kitchen,repairProblem, doorHangingOff, "A UPRN");

            // Assert
            await act.Should().ThrowExactlyAsync<T>();
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentTestData))]
#pragma warning disable xUnit1026
        public async void GivenAnInvalidRepairIssue_WhenExecute_ThenExceptionIsThrown<T>(T exception, string repairIssue) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange
            var systemUnderTest = new RetrieveAvailableAppointmentsUseCase(appointmentsGatewayMock.Object, sorEngineMock.Object);

            // Act
            Func<Task> act = async () => await systemUnderTest.Execute(kitchen, cupboards, repairIssue, "A UPRN");

            // Assert
            await act.Should().ThrowExactlyAsync<T>();
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentTestData))]
#pragma warning disable xUnit1026
        public async void GivenAnInvalidUprn_WhenExecute_ThenExceptionIsThrown<T>(T exception, string uprn) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange
            var systemUnderTest = new RetrieveAvailableAppointmentsUseCase(appointmentsGatewayMock.Object, sorEngineMock.Object);

            // Act
            Func<Task> act = async () => await systemUnderTest.Execute(kitchen, cupboards,doorHangingOff, uprn);

            // Assert
            await act.Should().ThrowExactlyAsync<T>();
        }

        public static IEnumerable<object[]> InvalidArgumentTestData()
        {
            yield return new object[] { new ArgumentNullException(), null };
            yield return new object[] { new ArgumentException(), "" };
            yield return new object[] { new ArgumentException(), " " };
        }

        [Fact]
        public async void GivenRepairParameters_WhenExecute_ThenMapSorCodeIsCalled()
        {
            await sytemUndertest.Execute(kitchen, cupboards,doorHangingOff, "uprn");
            sorEngineMock.Verify(x => x.MapSorCode(kitchen, cupboards, doorHangingOff), Times.Once);
        }

        [Fact]
        public async void GivenRepairParameters_WhenExecute_ThenGetAvailableAppointmentsGatewayIsCalled()
        {
            var repairCode = "N373049";
            sorEngineMock.Setup(x => x.MapSorCode(kitchen, cupboards, doorHangingOff)).Returns(repairCode);
            await sytemUndertest.Execute(kitchen, cupboards,doorHangingOff, "uprn");
            appointmentsGatewayMock.Verify(x => x.GetAvailableAppointments(repairCode, "uprn"), Times.Once);
        }
    }
}
