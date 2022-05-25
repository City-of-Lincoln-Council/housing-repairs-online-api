﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HACT.Dtos;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Domain.Boundaries;
using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.Mappers;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.UseCasesTests;

public class MigrationToRepairHubUseCaseTests
{
    private readonly MigrationToRepairHubUseCase systemUnderTest;
    private readonly Mock<IRepairsHubGateway> repairsHubGateway;
    private readonly Mock<IMapRepairsOnlineToRepairsHub> mapRepairsOnlineToRepairsHub;

    public MigrationToRepairHubUseCaseTests()
    {
        repairsHubGateway = new Mock<IRepairsHubGateway>();
        mapRepairsOnlineToRepairsHub = new Mock<IMapRepairsOnlineToRepairsHub>();
        systemUnderTest =
            new MigrationToRepairHubUseCase(repairsHubGateway.Object, mapRepairsOnlineToRepairsHub.Object);
    }

    [Fact]
#pragma warning disable xUnit1026
    public async void GivenANullRepairRequest_WhenExecute_ThenArgumentNullExceptionIsThrown()
#pragma warning restore xUnit1026
    {
        //Arrange
        var repair = new Repair();

        // Act
        Func<Task> act = async () => await systemUnderTest.Execute(null);

        // Assert
        await act.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
#pragma warning disable xUnit1026
    public async void GivenValidParameters_WhenExecute_ThenMapperIsCalled()
#pragma warning restore xUnit1026
    {
        //Arrange
        var repairRequest = new RepairRequest();
        var repair = new Repair();
        var repairsHubCreationRequest = new RepairsHubCreationRequest();

        mapRepairsOnlineToRepairsHub.Setup(x => x.Map(repairRequest)).Returns(repairsHubCreationRequest);

        // Act
        var result = await systemUnderTest.Execute(repairRequest);

        // Assert
        mapRepairsOnlineToRepairsHub.Verify(x => x.Map(repairRequest), Times.Once);
    }

    [Fact]
#pragma warning disable xUnit1026
    public async void GivenValidParameters_WhenExecute_ThenRepairsHubGatewayIsCalled()
#pragma warning restore xUnit1026
    {
        //Arrange
        var repairRequest = new RepairRequest();
        var repair = new Repair();
        var repairsHubCreationRequest = new RepairsHubCreationRequest();

        mapRepairsOnlineToRepairsHub.Setup(x => x.Map(repairRequest)).Returns(repairsHubCreationRequest);
        repairsHubGateway.Setup(x => x.CreateWorkOrder(repairsHubCreationRequest))
            .ReturnsAsync(new CreateWorkOrderResponse { Succeeded = true });

        // Act
        var result = await systemUnderTest.Execute(repairRequest);

        // Assert
        repairsHubGateway.Verify(x => x.CreateWorkOrder(repairsHubCreationRequest), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
#pragma warning disable xUnit1026
    public async void GivenMigrationSucceeded_WhenExecute_ThenItReturnsTrue(bool createWorkOrderResult)
#pragma warning restore xUnit1026
    {
        //Arrange
        var repairRequest = new RepairRequest();
        var repair = new Repair();
        var repairsHubCreationRequest = new RepairsHubCreationRequest();

        mapRepairsOnlineToRepairsHub.Setup(x => x.Map(repairRequest)).Returns(repairsHubCreationRequest);
        var actualCreateWorkOrderResponse = new CreateWorkOrderResponse { Succeeded = createWorkOrderResult };
        repairsHubGateway.Setup(x => x.CreateWorkOrder(repairsHubCreationRequest)).ReturnsAsync(actualCreateWorkOrderResponse);

        // Act
        var result = await systemUnderTest.Execute(repairRequest);

        // Assert
        result.Should().Be(actualCreateWorkOrderResponse);
    }
}
