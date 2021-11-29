using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.UseCases;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.UseCasesTests
{
    public class RetrieveAvailableAppointmentsUseCaseTests
    {
        [Theory]
        [MemberData(nameof(InvalidArgumentTestData))]
#pragma warning disable xUnit1026
        public async void GivenAnInvalidIdentifier_WhenConstructing_ThenExceptionIsThrown<T>(T exception, string repairCode) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange
            var systemUnderTest = new RetrieveAvailableAppointmentsUseCase();

            // Act
            Func<Task> act = async () => await systemUnderTest.Execute(repairCode, "A UPRN");

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
}
