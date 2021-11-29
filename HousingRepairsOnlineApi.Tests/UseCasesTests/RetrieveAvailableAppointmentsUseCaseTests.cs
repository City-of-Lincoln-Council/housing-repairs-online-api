using System;
using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.UseCases;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.UseCasesTests
{
    public class RetrieveAvailableAppointmentsUseCaseTests
    {
        [Fact]
        public async void GivenNullRepairCode_WhenExecute_ThenArgumentNullExceptionIsThrown()
        {
            // Arrange
            var systemUnderTest = new RetrieveAvailableAppointmentsUseCase();

            // Act
            Func<Task> act = async () => await systemUnderTest.Execute(null, "A UPRN");

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
