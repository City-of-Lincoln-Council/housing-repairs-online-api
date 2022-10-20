﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Controllers;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.ControllersTests
{
    public class AppointmentsControllerTests : ControllerTests
    {
        private const string RepairLocation = "kitchen";
        private const string RepairProblem = "cupboards";
        private const string RepairIssue = "doorHangingOff";
        private const string LocationId = "location ID";

        private AppointmentsController systemUndertest;
        private Mock<IRetrieveAvailableAppointmentsUseCase> availableAppointmentsUseCaseMock;
        public AppointmentsControllerTests()
        {
            availableAppointmentsUseCaseMock = new Mock<IRetrieveAvailableAppointmentsUseCase>();
            systemUndertest = new AppointmentsController(availableAppointmentsUseCaseMock.Object);
        }

        [Fact]
        public async Task TestEndpoint()
        {
            var result = await systemUndertest.AvailableAppointments(RepairType.Tenant, RepairLocation, RepairProblem, RepairIssue, LocationId);
            GetStatusCode(result).Should().Be(200);
            availableAppointmentsUseCaseMock.Verify(x => x.Execute(It.IsAny<string>(), RepairLocation, RepairProblem, RepairIssue, LocationId, null), Times.Once);
        }

        [Fact]
        public async Task TestTenantEndpoint()
        {
            // Arrange

            // Act
            _ = await systemUndertest.AvailableTenantAppointments(RepairLocation, RepairProblem, RepairIssue, LocationId);

            // Assert
            availableAppointmentsUseCaseMock.Verify(
                x => x.Execute(RepairType.Tenant, RepairLocation, RepairProblem, RepairIssue, LocationId, null), Times.Once);
        }

        [Fact]
        public async Task GivenAFromDate_WhenRequestingAvailableAppointments_ThenResultsAreReturned()
        {
            // Arrange

            // Act
            var result = await systemUndertest.AvailableAppointments(RepairType.Tenant, RepairLocation, RepairProblem, RepairIssue, LocationId);

            // Assert
            GetStatusCode(result).Should().Be(200);
            availableAppointmentsUseCaseMock.Verify(x => x.Execute(It.IsAny<string>(), RepairLocation, RepairProblem, RepairIssue, LocationId, null), Times.Once);
        }

        [Fact]
        public async Task ReturnsErrorWhenFailsToSave()
        {
            var fromDate = new DateTime(2021, 12, 15);

            availableAppointmentsUseCaseMock.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Throws<Exception>();

            var result = await systemUndertest.AvailableAppointments(RepairType.Tenant, RepairLocation, RepairProblem, RepairIssue, LocationId, fromDate);

            GetStatusCode(result).Should().Be(500);
        }
    }
}
