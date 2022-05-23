using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using HousingRepairsOnlineApi.Domain.Boundaries;
using HousingRepairsOnlineApi.Domain.Boundaries.RepairsHub;
using HousingRepairsOnlineApi.Gateways;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.GatewaysTests;

public class RepairsHubGatewayTests
{
    private RepairsHubGateway systemUnderTest;
    private readonly MockHttpMessageHandler mockHttp;
    private const string RepairsHubApiEndpoint = "https://repairshub.api";
    private const string WorkOrdersUri = $"/api/v2/workOrders/schedule";

    public RepairsHubGatewayTests()
    {
        mockHttp = new MockHttpMessageHandler();
        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri(RepairsHubApiEndpoint);

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        systemUnderTest = new RepairsHubGateway(httpClientFactoryMock.Object);
    }

    [Fact]
    public async void GivenNullRepairsHubCreationRequest_WhenCreatingWorkOrder_ThenArgumentNullExceptionIsThrown()
    {
        // Arrange

        //Act

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => systemUnderTest.CreateWorkOrder(null));
    }

    [Fact]
    public async void GivenARepairsHubCreationRequest_WhenCreatingWorkOrder_ThenAWorkOrderRequestIsSent()
    {
        // Arrange
        var stringContent = @"{""Reference"":[{""Id"":""9925829f-fd0e-4272-8677-9041dac2894d""}],""DescriptionOfWork"":""description text"",""Priority"":{""PriorityCode"":4,""PriorityDescription"":""5 [N] NORMAL"",""RequiredCompletionDateTime"":""0001-01-01T23:59:00-00:01"",""NumberOfDays"":21},""WorkClass"":{""WorkClassCode"":0},""WorkElement"":[{""RateScheduleItem"":[{""CustomCode"":""20110200"",""CustomName"":""EMERGENCY LIGHTING TEST DWELL"",""Quantity"":{""Amount"":[1]}}],""Trade"":null}],""Site"":{""Property"":[{""PropertyReference"":""12345678"",""Address"":{""AddressLine"":[""12 Pitcairn House St Thomass Square""],""PostalCode"":""E9 6PT""},""Reference"":[{""Id"":""12345678""}]}]},""InstructedBy"":{""Name"":""Hackney Housing""},""AssignedToPrimary"":{""Name"":""HH General Building Repai"",""Organization"":{""Reference"":[{""Id"":""H01""}]}},""Customer"":{""Name"":""contact name"",""Person"":{""Name"":{""Full"":""contact name""},""Communication"":[{""Channel"":{""Medium"":""20"",""Code"":""60""},""Value"":""07000000000""}]}},""BudgetCode"":{""Id"":""8""},""MultiTradeWorkOrder"":false}";
        mockHttp.Expect(WorkOrdersUri).WithContent(stringContent).Respond(HttpStatusCode.OK);

        var repairsHubCreationRequest = new RepairsHubCreationRequest
        {
            Reference = new List<Reference>{new Reference{Id = "9925829f-fd0e-4272-8677-9041dac2894d"}},
            DescriptionOfWork = "description text",
            Priority = new Domain.Boundaries.RepairsHub.Priority
            {
                PriorityCode = 4,
                PriorityDescription = "5 [N] NORMAL",
                RequiredCompletionDateTime = DateTime.Parse("0001-01-01T00:00:00.000Z", CultureInfo.InvariantCulture),
                NumberOfDays = 21
            },
            WorkClass = new Domain.Boundaries.RepairsHub.WorkClass
            {
                WorkClassCode = 0
            },
            BudgetCode = new Domain.Boundaries.RepairsHub.BudgetCode
            {
                Id = "8"
            },
            MultiTradeWorkOrder = false,
            Site = new Domain.Boundaries.RepairsHub.Site
            {
                Property = new List<Domain.Boundaries.RepairsHub.Property>
                {
                    new Domain.Boundaries.RepairsHub.Property
                    {
                        PropertyReference = "12345678",
                        Address = new Domain.Boundaries.RepairsHub.Address
                        {
                            AddressLine = new List<string>{"12 Pitcairn House St Thomass Square"},
                            PostalCode = "E9 6PT"
                        },
                        Reference = new List<Reference>
                        {
                            new Reference{ Id = "12345678"}
                        }
                    }
                }
            },
            AssignedToPrimary = new Domain.Boundaries.RepairsHub.AssignedToPrimary
            {
                Name = "HH General Building Repai",
                Organization = new Domain.Boundaries.RepairsHub.Organization
                {
                    Reference = new List<Domain.Boundaries.RepairsHub.Reference>
                    {
                        new Domain.Boundaries.RepairsHub.Reference
                        {
                            Id = "H01"
                        }
                    }
                }
            },
            InstructedBy = new Domain.Boundaries.RepairsHub.InstructedBy
            {
                Name = "Hackney Housing"
            },
            Customer = new Domain.Boundaries.RepairsHub.Customer
            {
                Name = "contact name",
                Person = new Domain.Boundaries.RepairsHub.Person
                {
                    Name = new Domain.Boundaries.RepairsHub.Name
                    {
                        Full = "contact name"
                    },
                    Communication = new List<Domain.Boundaries.RepairsHub.Communication>
                    {
                        new Domain.Boundaries.RepairsHub.Communication
                        {
                           Value = "07000000000",
                           Channel = new Domain.Boundaries.RepairsHub.Channel
                           {
                               Code = "60",
                               Medium = "20"
                           }
                        }
                    }
                }
            },
            WorkElement = new List<Domain.Boundaries.RepairsHub.WorkElement>
            {
                new Domain.Boundaries.RepairsHub.WorkElement
                {
                    RateScheduleItem = new List<Domain.Boundaries.RepairsHub.RateScheduleItem>
                    {
                        new Domain.Boundaries.RepairsHub.RateScheduleItem
                        {
                            Quantity = new Domain.Boundaries.RepairsHub.Quantity
                            {
                                Amount = new List<int>{ 1 }
                            },
                            CustomCode = "20110200",
                            CustomName = "EMERGENCY LIGHTING TEST DWELL"
                        }
                    }
                }
            }
        };

        // Act
        _ = await systemUnderTest.CreateWorkOrder(repairsHubCreationRequest);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }


    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.Forbidden, false)]
    public async void GivenARepairsHubCreationRequest_WhenCreatingWorkOrderAndResponseHasSuccessfulStatusCode_ThenTrueIsReturned(HttpStatusCode httpStatusCode, bool expected)
    {
        // Arrange
        mockHttp.When(WorkOrdersUri).Respond(httpStatusCode);

        // Act
        var response = await systemUnderTest.CreateWorkOrder(new RepairsHubCreationRequest());

        // Assert
        Assert.Equal(expected, response);
    }
}
