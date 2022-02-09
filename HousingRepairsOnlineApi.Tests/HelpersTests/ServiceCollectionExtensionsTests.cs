using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using HousingRepairsOnlineApi.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;
using ServiceCollectionExtensions = HousingRepairsOnlineApi.Helpers.ServiceCollectionExtensions;

namespace HousingRepairsOnlineApi.Tests.HelpersTests
{
    public class ServiceCollectionExtensionsTests
    {
        private readonly MockFileSystem fileSystem = new();

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

            var jsonConfig = @"{
                ""kitchen"": {
                        ""cupboards"": {
                        ""doorHangingOff"": ""SoR Code 1""
                    }
                },
                ""bathroom"": {
                    ""bath"": {
                        ""taps"": ""SoR Code 1""
                    }
                }
            }";
            const string configFilePath = "SoRConfig.json";
            fileSystem.AddFile(configFilePath, new MockFileData(jsonConfig));

            // Act
            ServiceCollectionExtensions.AddSoREngine(serviceCollection, configFilePath, fileSystem);

            // Assert
            serviceCollectionMock.VerifyAll();
        }

        [Theory]
        [MemberData(nameof(InvalidJsonSorConfigTestData))]
#pragma warning disable xUnit1026
        public void GivenInvalidJsonSorConfig_WhenAddingSoREngineToServices_ThenExceptionIsThrown<T>(T expection, string configJson) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var serviceCollection = serviceCollectionMock.Object;

            const string configFilePath = "SoRConfig.json";
            fileSystem.AddFile(configFilePath, new MockFileData(configJson));

            // Act
            Action act = () => ServiceCollectionExtensions.AddSoREngine(serviceCollection, configFilePath, fileSystem);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithInnerException<T>();
        }

        public static IEnumerable<object[]> InvalidJsonSorConfigTestData()
        {
            yield return new object[] { new JsonSerializationException(), @"{" };
            yield return new object[] { new JsonReaderException(), @"}" };
            yield return new object[] { new JsonSerializationException(), @"
                {
                    ""kitchen"": {
                }"
            };
            yield return new object[] { new JsonReaderException(), @"
                {
                    ""kitchen"": {
                            ""cupboards"": {
                            ""doorHangingOff"": ""SoR Code 1""
                        }
                    ""bathroom"": {
                        ""bath"": {
                            ""taps"": ""SoR Code 1""
                        }
                    }
                }"
            };

        }

        [Fact]
        public void GivenPathToMissingSorConfigPathArgument_WhenAddingSoREngineToServices_ThenInvalidOperationExceptionIsThrown()
        {
            // Arrange
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var serviceCollection = serviceCollectionMock.Object;

            // Act
            Action act = () => ServiceCollectionExtensions.AddSoREngine(serviceCollection, "SoRConfig.missing.json", fileSystem);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithInnerException<FileNotFoundException>();
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentTestData))]
#pragma warning disable xUnit1026
        public void GivenInvalidSorConfigPathArgument_WhenAddingSoREngineToServices_ThenExceptionIsThrown<T>(T exception, string sorConfigPath) where T : Exception
#pragma warning restore xUnit1026
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
