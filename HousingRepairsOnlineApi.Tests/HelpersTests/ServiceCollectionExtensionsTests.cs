using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using HousingRepairsOnlineApi.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.HelpersTests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void GivenValidSorConfigPathArgument_WhenAddingSoREngineToServices_ThenSoREngineIsRegistered()
        {
            // Arrange
            var serviceCollectionMock = new Mock<IServiceCollection>();
            serviceCollectionMock.Setup(x =>
                x.Add(It.Is<ServiceDescriptor>(serviceDescriptor =>
                        serviceDescriptor.ServiceType == typeof(ISoREngine) &&
                        serviceDescriptor.ImplementationFactory != null
                    )
                )
            );
            var serviceCollection = serviceCollectionMock.Object;

            // Act
            ServiceCollectionExtensions.AddSoREngine(serviceCollection,
                "HelpersTests/ServiceCollectionExtensionsTestFiles/SoRConfig.json");

            // Assert
            serviceCollectionMock.VerifyAll();
        }

        [Fact]
        public void GivenInvalidSorConfigPathArgument_WhenAddingSoREngineToServices_ThenInvalidOperationExceptionIsThrown()
        {
            // Arrange
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var serviceCollection = serviceCollectionMock.Object;

            // Act
            Action act = () => ServiceCollectionExtensions.AddSoREngine(serviceCollection, "HelpersTests/ServiceCollectionExtensionsTestFiles/SoRConfig.invalid.json");

            // Assert
            act.Should().Throw<InvalidOperationException>().WithInnerException<JsonReaderException>();
        }

        [Fact]
        public void GivenPathToMissingSorConfigPathArgument_WhenAddingSoREngineToServices_ThenInvalidOperationExceptionIsThrown()
        {
            // Arrange
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var serviceCollection = serviceCollectionMock.Object;

            // Act
            Action act = () => ServiceCollectionExtensions.AddSoREngine(serviceCollection, "HelpersTests/ServiceCollectionExtensionsTestFiles/SoRConfig.missing.json");

            // Assert
            act.Should().Throw<InvalidOperationException>().WithInnerException<FileNotFoundException>();
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentTestData))]
        public void GivenInvalidSorConfigPathArgument_WhenAddingSoREngineToServices_ThenExceptionIsThrown<T>(T exception, string sorConfigPath) where T:Exception
        {
            // Arrange
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var serviceCollection = serviceCollectionMock.Object;

            // Act
            Action act = () => ServiceCollectionExtensions.AddSoREngine(serviceCollection, sorConfigPath);

            // Assert
            act.Should().Throw<T>();
        }

        public static IEnumerable<object[]> InvalidArgumentTestData()
        {
            yield return new object[] { new ArgumentNullException(), null };
            yield return new object[] { new ArgumentException(), "" };
            yield return new object[] { new ArgumentException(), " " };
        }
    }
}
