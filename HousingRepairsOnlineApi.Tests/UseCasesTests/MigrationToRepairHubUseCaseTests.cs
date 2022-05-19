using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HACT.Dtos;
using HousingRepairsOnlineApi.Domain;
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
        repairsHubGateway= new Mock<IRepairsHubGateway>();
        mapRepairsOnlineToRepairsHub = new Mock<IMapRepairsOnlineToRepairsHub>();
        systemUnderTest = new MigrationToRepairHubUseCase(repairsHubGateway.Object,mapRepairsOnlineToRepairsHub.Object);
    }

    [Theory]
    [MemberData(nameof(InvalidArgumentTestData))]
#pragma warning disable xUnit1026
    public async void GivenAnInvalidRepairLocation_WhenExecute_ThenExceptionIsThrown<T>(T exception, string token) where T : Exception
#pragma warning restore xUnit1026
    {
        //Arrange
        var repairRequest = new RepairRequest();
        var repair = new Repair();
        // Act
        Func<Task> act = async () => await systemUnderTest.Execute(repairRequest,repair, token);

        // Assert
        await act.Should().ThrowExactlyAsync<T>();
    }
    public static IEnumerable<object[]> InvalidArgumentTestData()
    {
        yield return new object[] { new ArgumentNullException(), null };
        yield return new object[] { new ArgumentException(), "" };
        yield return new object[] { new ArgumentException(), " " };
    }
}
