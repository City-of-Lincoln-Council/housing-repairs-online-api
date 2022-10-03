﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Xunit;
using ServiceCollectionExtensions = HousingRepairsOnlineApi.Helpers.ServiceCollectionExtensions;

namespace HousingRepairsOnlineApi.Tests.HelpersTests
{
    public class ServiceCollectionExtensionsTests
    {
        private readonly Mock<ISorConfigurationProvider> sorConfigurationProviderMock = new();

        private const string Value1 = "value1";
        private const string Display1 = "display1";
        private const string Value2 = "value2";
        private const string Display2 = "display2";
        private const string Value3 = "value3";
        private const string Display3 = "display3";
        private const string SorCode = "sorCode";

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

            var jsonConfig = @"[{
                ""value"": ""kitchen"",
                ""display"": ""Kitchen"",
                ""options"": [{
                        ""value"": ""cupboards"",
                        ""display"": ""Cupboards, including damaged..."",
                        ""options"": [{
                            ""value"": ""doorHangingOff"",
                            ""display"": ""Hanging Door"",
                            ""sorCode"": ""SoR Code 1""
                        }]
                    }]
                },
                {
                ""value"": ""bathroom"",
                ""display"": ""Bathroom"",
                ""options"": [{
                    ""value"": ""bath"",
                    ""display"": ""Bath, including taps"",
                    ""options"": [{
                        ""value"": ""taps"",
                        ""display"": ""Bath taps"",
                        ""sorCode"": ""SoR Code 1""
                    }]
                }]
            }]";
            sorConfigurationProviderMock.Setup(x => x.ConfigurationValue()).Returns(jsonConfig);

            // Act
            ServiceCollectionExtensions.AddSoREngine(serviceCollection, sorConfigurationProviderMock.Object);

            // Assert
            serviceCollectionMock.VerifyAll();
        }

        [Theory]
        [MemberData(nameof(InvalidJsonSorConfigTestData))]
#pragma warning disable xUnit1026
        public void GivenInvalidJsonSorConfig_WhenAddingSoREngineToServices_ThenExceptionIsThrown<T>(T exception,
            string configJson) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var serviceCollection = serviceCollectionMock.Object;

            sorConfigurationProviderMock.Setup(x => x.ConfigurationValue()).Returns(configJson);

            // Act
            Action act = () =>
                ServiceCollectionExtensions.AddSoREngine(serviceCollection, sorConfigurationProviderMock.Object);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithInnerException<T>();
        }

        [Theory]
        [MemberData(nameof(InvalidJsonSorConfigTestData))]
#pragma warning disable xUnit1026
        public void GivenInvalidJsonSorConfig_WhenParsingSorConfigurationJson_ThenExceptionIsThrown<T>(T exception,
            string configJson) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange

            // Act
            Action act = () => ServiceCollectionExtensions.ParseSorConfigurationJson(configJson);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithInnerException<T>();
        }

        [Theory]
        [MemberData(nameof(InvalidAgainstSchemaJsonAppointmentSlotConfigTestData))]
        [MemberData(nameof(InvalidJsonAppointmentSlotConfigTestData))]
#pragma warning disable xUnit1026
        public void GivenInvalidJsonAppointmentSlotsConfiguration_WhenParsingConfigurationJson_ThenExceptionIsThrown<T>(
            T exception,
            string configJson) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange

            // Act
            Action act = () => ServiceCollectionExtensions.ParseAppointmentSlotsConfigurationJson(configJson);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithInnerException<T>();
        }

        [Theory]
        [MemberData(nameof(ValidJsonAppointmentSlotConfigTestData))]
#pragma warning disable xUnit1026
        public void GivenValidJsonAppointmentSlotsConfiguration_WhenParsingConfigurationJson_ThenExceptionIsThrown(
            string configJson, IEnumerable<AppointmentSlotTimeSpan> expected)
#pragma warning restore xUnit1026
        {
            // Arrange

            // Act
            var actual = ServiceCollectionExtensions.ParseAppointmentSlotsConfigurationJson(configJson);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(ValidSorConfigurationForCreatingJourneyRepairTriageOptions))]
        public void GivenSorConfigurations_WhenGeneratingJourneyRepairTriageOptions_ThenRepairTriageOptionsReturned(
            IEnumerable<SorConfiguration> sorConfigurations, IEnumerable<RepairTriageOption> expected)
        {
            // Arrange

            // Act
            var actual = ServiceCollectionExtensions.GenerateJourneyRepairTriageOptions(
                sorConfigurations);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(ValidSorConfigurationForCreatingSorMapping))]
        public void GivenSorConfigurations_WhenGeneratingSorMapping_ThenCorrectMappingCreated(
            IEnumerable<SorConfiguration> sorConfigurations, IDictionary<string, IDictionary<string, dynamic>> expected)
        {
            // Arrange

            // Act
            var actual = ServiceCollectionExtensions.GenerateSorMapping(sorConfigurations);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        public static TheoryData<JsonException, string> InvalidJsonSorConfigTestData()
        {
            return new TheoryData<JsonException, string>
            {
                { new JsonSerializationException(), @"{" },
                { new JsonReaderException(), @"}" },
                {
                    new JsonSerializationException(), @"{
                        ""kitchen"": {
                    }"
                },
                {
                    new JsonReaderException(), @"[{
                    ""value"": ""kitchen"",
                    ""display"": ""Kitchen"",
                    ""options"": [{
                            ""value"": ""cupboards"",
                            ""display"": ""Cupboards, including damaged..."",
                            ""options"": [{
                                ""value"": ""doorHangingOff"",
                                ""display"": ""Hanging Door"",
                                ""sorCode"": ""SoR Code 1""
                            }]
                        }]
                    {
                    ""value"": ""bathroom"",
                    ""display"": ""Bathroom"",
                    ""options"": [{
                        ""value"": ""bath"",
                        ""display"": ""Bath, including taps"",
                        ""options"": [{
                            ""value"": ""taps"",
                            ""display"": ""Bath taps"",
                            ""sorCode"": ""SoR Code 1""
                        }]
                    }]
                }]"
                },
            };
        }

        public static TheoryData<IEnumerable<SorConfiguration>, IEnumerable<RepairTriageOption>> ValidSorConfigurationForCreatingJourneyRepairTriageOptions()
        {
            return new TheoryData<IEnumerable<SorConfiguration>, IEnumerable<RepairTriageOption>>()
            {
                { Array.Empty<SorConfiguration>(), Array.Empty<RepairTriageOption>() },
                {
                    new[] { new SorConfiguration { Value = Value1, Display = Display1, SorCode = SorCode } },
                    new[] { new RepairTriageOption { Value = Value1, Display = Display1, } }
                },
                {
                    new[]
                    {
                        new SorConfiguration
                        {
                            Value = Value1,
                            Display = Display1,
                            Options = new[]
                            {
                                new SorConfiguration
                                {
                                    Value = Value2, Display = Display2, SorCode = SorCode
                                }
                            }
                        }
                    },
                    new[]
                    {
                        new RepairTriageOption
                        {
                            Value = Value1,
                            Display = Display1,
                            Options = new[] { new RepairTriageOption { Value = Value2, Display = Display2 } }
                        }
                    }
                },
                {
                    new[]
                    {
                        new SorConfiguration
                        {
                            Value = Value1,
                            Display = Display1,
                            Options = new[]
                            {
                                new SorConfiguration
                                {
                                    Value = Value2,
                                    Display = Display2,
                                    Options = new[]
                                    {
                                        new SorConfiguration
                                        {
                                            Value = Value3,
                                            Display = Display3,
                                            SorCode = SorCode
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new[]
                    {
                        new RepairTriageOption
                        {
                            Value = Value1,
                            Display = Display1,
                            Options = new[]
                            {
                                new RepairTriageOption
                                {
                                    Value = Value2,
                                    Display = Display2,
                                    Options = new[]
                                    {
                                        new RepairTriageOption
                                        {
                                            Value = Value3, Display = Display3
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
            };
        }

        public static TheoryData<IEnumerable<SorConfiguration>, IDictionary<string, IDictionary<string, dynamic>>>
            ValidSorConfigurationForCreatingSorMapping()
        {
            return new TheoryData<IEnumerable<SorConfiguration>, IDictionary<string, IDictionary<string, dynamic>>>
            {
                { Array.Empty<SorConfiguration>(), new Dictionary<string, IDictionary<string, dynamic>>() },
                {
                    new[]
                    {
                        new SorConfiguration
                        {
                            Value = Value1,
                            Display = Display1,
                            Options = new[]
                            {
                                new SorConfiguration
                                {
                                    Value = Value2, Display = Display2, SorCode = SorCode
                                }
                            }
                        }
                    },
                    new Dictionary<string, IDictionary<string, dynamic>>
                    {
                        { Value1, new Dictionary<string, dynamic> { { Value2, SorCode } } }
                    }
                },
                {
                    new[]
                    {
                        new SorConfiguration
                        {
                            Value = Value1,
                            Display = Display1,
                            Options = new[]
                            {
                                new SorConfiguration
                                {
                                    Value = Value2,
                                    Display = Display2,
                                    Options = new[]
                                    {
                                        new SorConfiguration
                                        {
                                            Value = Value3,
                                            Display = Display3,
                                            SorCode = SorCode
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new Dictionary<string, IDictionary<string, dynamic>>
                    {
                        {
                            Value1,
                            new Dictionary<string, dynamic>
                            {
                                { Value2, new Dictionary<string, string> { { Value3, SorCode } } }
                            }
                        }
                    }
                },
            };
        }

        public static TheoryData<JSchemaValidationException, string> InvalidAgainstSchemaJsonAppointmentSlotConfigTestData()
        {
            return new TheoryData<JSchemaValidationException, string>
            {
                { new JSchemaValidationException(), @"{" },
                {
                    new JSchemaValidationException(), @"{
                        ""startTime"": {
                    }"
                },
                {
                    new JSchemaValidationException(), @"[{
                        ""startTime"": ""08:00:00""
                    }]"
                },
                {
                    new JSchemaValidationException(), @"[{
                        ""endTime"": ""08:00:00""
                    }]"
                },
                {
                    new JSchemaValidationException(), @"[{}]"
                },
            };
        }

        public static TheoryData<JsonException, string> InvalidJsonAppointmentSlotConfigTestData()
        {
            return new TheoryData<JsonException, string>
            {
                { new JsonReaderException(), @"}" },
                {
                    new JsonReaderException(), @"[{
                    ""startTime"": ""08:00:00""
                    ]"
                },
            };
        }

        public static TheoryData<string, IEnumerable<AppointmentSlotTimeSpan>> ValidJsonAppointmentSlotConfigTestData()
        {
            return new TheoryData<string, IEnumerable<AppointmentSlotTimeSpan>>
            {
                {
                    @"[
                        {
                          ""startTime"": ""08:00:00"",
                          ""endTime"": ""12:00:00""
                        }
                    ]",
                    new AppointmentSlotTimeSpan[]
                    {
                        new() { StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0) }
                    }
                },
                {
                    @"[
                        {
                          ""startTime"": ""08:00:00"",
                          ""endTime"": ""12:00:00""
                        },
                        {
                          ""startTime"": ""12:00:00"",
                          ""endTime"": ""16:00:00""
                        }
                    ]",
                    new AppointmentSlotTimeSpan[]
                    {
                        new() { StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0) },
                        new() { StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(16, 0, 0) },
                    }
                },
            };
        }
    }
}
