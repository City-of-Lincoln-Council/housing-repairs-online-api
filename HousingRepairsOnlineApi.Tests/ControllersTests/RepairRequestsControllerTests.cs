﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Controllers;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Domain.Boundaries;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests
{
    public class RepairRequestsControllerTests : ControllerTests
    {
        private const string repairId = "1AB2C3D4";
        private RepairController systemUnderTest;
        private Mock<ISaveRepairRequestUseCase> saveRepairRequestUseCaseMock;
        private Mock<IBookAppointmentUseCase> bookAppointmentUseCaseMock;
        private Mock<IInternalEmailSender> internalEmailSender;
        private Mock<IAppointmentConfirmationSender> appointmentConfirmationSender;
        private Mock<IMigrationToRepairHubUseCase> migrationToRepairHubUseCase;

        private readonly RepairAvailability repairAvailability = new()
        {
            Display = "Displayed Time",
            StartDateTime = new DateTime(2022, 01, 01, 8, 0, 0),
            EndDateTime = new DateTime(2022, 01, 01, 12, 0, 0),
        };

        private readonly RepairAddress repairAddress = new()
        {
            LocationId = "Location Id",
        };

        public RepairRequestsControllerTests()
        {
            saveRepairRequestUseCaseMock = new Mock<ISaveRepairRequestUseCase>();
            bookAppointmentUseCaseMock = new Mock<IBookAppointmentUseCase>();
            bookAppointmentUseCaseMock.Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>())).ReturnsAsync(new SchedulingApiBookingResponse());
            appointmentConfirmationSender = new Mock<IAppointmentConfirmationSender>();
            internalEmailSender = new Mock<IInternalEmailSender>();
            migrationToRepairHubUseCase = new Mock<IMigrationToRepairHubUseCase>();
            migrationToRepairHubUseCase
                .Setup(x => x.Execute(It.IsAny<RepairRequest>()))
                .ReturnsAsync(new CreateWorkOrderResponse{Succeeded = true, Id = repairId});
            systemUnderTest = new RepairController(saveRepairRequestUseCaseMock.Object, internalEmailSender.Object,
                appointmentConfirmationSender.Object, bookAppointmentUseCaseMock.Object,
                migrationToRepairHubUseCase.Object);
        }

        [Fact]
        public async Task TestEndpoint()
        {
            var repairRequest = new RepairRequest
            {
                ContactDetails = new RepairContactDetails { Value = "07465087654" },
                Address = new RepairAddress { Display = "address", LocationId = "uprn" },
                Description = new RepairDescriptionRequest { Text = "repair description", Base64Img = "image" },
                Location = new RepairLocation { Value = "location" },
                Problem = new RepairProblem { Value = "problem" },
                Issue = new RepairIssue { Value = "issue" }
            };

            var repair = new Repair
            {
                Id = repairId,
                ContactDetails = new RepairContactDetails { Value = "07465087654" },
                Address = new RepairAddress { Display = "address", LocationId = "uprn" },
                Description = new RepairDescription { Text = "repair description", Base64Image = "image", PhotoUrl = "x/Url.png" },
                Location = new RepairLocation { Value = "location" },
                Problem = new RepairProblem { Value = "problem" },
                Issue = new RepairIssue { Value = "issue" },
                SOR = "sor",
                Time = repairAvailability,
            };
            saveRepairRequestUseCaseMock.Setup(x => x.Execute(It.IsAny<RepairRequest>(), repairId)).ReturnsAsync(repair);

            var result = await systemUnderTest.SaveRepair(repairRequest);

            GetStatusCode(result).Should().Be(200);


            saveRepairRequestUseCaseMock.Verify(x => x.Execute(repairRequest, repairId), Times.Once);

            internalEmailSender.Verify(x => x.Execute(repair), Times.Once);
        }

        [Fact]
        public async Task ReturnsErrorWhenFailsToSave()
        {
            RepairRequest repairRequest = new RepairRequest();

            saveRepairRequestUseCaseMock.Setup(x => x.Execute(It.IsAny<RepairRequest>(), repairId)).Throws<System.Exception>();

            var result = await systemUnderTest.SaveRepair(repairRequest);

            GetStatusCode(result).Should().Be(500);
            saveRepairRequestUseCaseMock.Verify(x => x.Execute(repairRequest, repairId), Times.Once);
        }

        [Fact]
        public async Task GivenEmailContact_WhenRepair_ThenSendAppointmentConfirmationEmailUseCaseIsCalled()
        {
            //Arrange
            RepairRequest repairRequest = new RepairRequest
            {
                ContactDetails = new RepairContactDetails
                {
                    Type = "email",
                    Value = "dr.who@tardis.com"
                },
                Time = new RepairAvailability
                {
                    Display = "Displayed Time"
                }
            };
            var repair = new Repair()
            {
                Id = repairId,
                ContactDetails = new RepairContactDetails
                {
                    Type = "email",
                    Value = "dr.who@tardis.com"
                },
                Time = repairAvailability,
                Address = repairAddress
            };

            saveRepairRequestUseCaseMock.Setup(x => x.Execute(repairRequest, repairId)).ReturnsAsync(repair);

            //Assert
            await systemUnderTest.SaveRepair(repairRequest);

            //Act
            appointmentConfirmationSender.Verify(x => x.Execute(repair), Times.Once);
        }

        [Fact]
        public async Task GivenSmsContact_WhenRepair_ThenSendAppointmentConfirmationSmsUseCaseIsCalled()
        {
            //Arrange
            var repairRequest = new RepairRequest()
            {
                ContactDetails = new RepairContactDetails
                {
                    Type = "text",
                    Value = "0765374057"
                },
                Time = new RepairAvailability
                {
                    Display = "Displayed Time"
                }
            };
            var repair = new Repair
            {
                Id = repairId,
                ContactDetails = new RepairContactDetails
                {
                    Type = "text",
                    Value = "0765374057"
                },
                Time = repairAvailability,
                Address = repairAddress
            };


            saveRepairRequestUseCaseMock.Setup(x => x.Execute(repairRequest, repairId)).ReturnsAsync(repair);

            //Act
            await systemUnderTest.SaveRepair(repairRequest);

            //Assert
            appointmentConfirmationSender.Verify(x => x.Execute(repair), Times.Once);
        }
    }
}
