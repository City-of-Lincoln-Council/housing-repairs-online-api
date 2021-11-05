using System;
using System.Collections.Generic;
using FluentAssertions;
using HousingRepairsOnlineApi.Helpers;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.HelpersTests
{
    public class IdentifierValidatorTests
    {
        [Theory]
        [MemberData(nameof(InvalidArgumentTestData))]
#pragma warning disable xUnit1026
        public void GivenAnInvalidIdentifier_WhenConstructing_ThenExceptionIsThrown<T>(T exception, string identifier) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange
            // Act
#pragma warning disable CA1806
            Action act = () => new IdentifierValidator(identifier);
#pragma warning restore CA1806

            // Assert
            act.Should().Throw<T>();
        }

        public static IEnumerable<object[]> InvalidArgumentTestData()
        {
            yield return new object[] { new ArgumentNullException(), null };
            yield return new object[] { new ArgumentException(), "" };
            yield return new object[] { new ArgumentException(), " " };
        }

        [Fact]
        public void GivenAValidIdentifier_WhenValidateIsCalled_ThenTrueIsReturned()
        {
            const string identifier = "M3";
            var systemUnderTest = new IdentifierValidator(identifier);
            var result = systemUnderTest.Validate(identifier);

            result.Should().Be(true);
        }

        [Fact]
        public void GivenAnInvalidIdentifier_WhenValidateIsCalled_ThenFalseIsReturned()
        {
            const string identifier = "M3";
            var systemUnderTest = new IdentifierValidator(identifier);
            var result = systemUnderTest.Validate("NotTheSameIdentifier");

            result.Should().Be(false);
        }
    }
}
