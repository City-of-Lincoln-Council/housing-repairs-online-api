﻿using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Controllers;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests
{
    public class RepairRequestsControllerTests : ControllerTests
    {
        private RepairController systemUnderTest;
        private Mock<ISaveRepairRequestUseCase> saveRepairRequestUseCaseMock;
        private Mock<ISendInternalEmailUseCase> sendInternalEmailUseCase;
        private Mock<IRetrieveImageLinkUseCase> retrieveImageLinkUseCase;
        private Mock<IAppointmentConfirmationSender> appointmentConfirmationSender;

        public RepairRequestsControllerTests()
        {
            saveRepairRequestUseCaseMock = new Mock<ISaveRepairRequestUseCase>();
            appointmentConfirmationSender = new Mock<IAppointmentConfirmationSender>();
            sendInternalEmailUseCase = new Mock<ISendInternalEmailUseCase>();
            retrieveImageLinkUseCase = new Mock<IRetrieveImageLinkUseCase>();
            systemUnderTest = new RepairController(saveRepairRequestUseCaseMock.Object, sendInternalEmailUseCase.Object, retrieveImageLinkUseCase.Object, appointmentConfirmationSender.Object);

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
                Id = "1AB2C3D4",
                ContactDetails = new RepairContactDetails { Value = "07465087654" },
                Address = new RepairAddress { Display = "address", LocationId = "uprn" },
                Description = new RepairDescription { Text = "repair description", Base64Image = "image", PhotoUrl = "x/Url.png" },
                Location = new RepairLocation { Value = "location" },
                Problem = new RepairProblem { Value = "problem" },
                Issue = new RepairIssue { Value = "issue" },
                SOR = "sor"
            };

            saveRepairRequestUseCaseMock.Setup(x => x.Execute(It.IsAny<RepairRequest>())).ReturnsAsync(repair);

            retrieveImageLinkUseCase.Setup(x => x.Execute(repair.Description.PhotoUrl)).ReturnsAsync("Url.png");

            var result = await systemUnderTest.SaveRepair(repairRequest);

            GetStatusCode(result).Should().Be(200);

            sendInternalEmailUseCase.Verify(x => x.Execute(
                repair.Id,
                repair.Address.LocationId,
                repair.Address.Display,
                repair.SOR,
                repair.Description.Text,
                repair.ContactDetails.Value,
                "Url.png"),
                Times.Once);

            saveRepairRequestUseCaseMock.Verify(x => x.Execute(repairRequest), Times.Once);

            retrieveImageLinkUseCase.Verify(x => x.Execute(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ReturnsErrorWhenFailsToSave()
        {
            RepairRequest repairRequest = new RepairRequest();

            saveRepairRequestUseCaseMock.Setup(x => x.Execute(It.IsAny<RepairRequest>())).Throws<System.Exception>();

            var result = await systemUnderTest.SaveRepair(repairRequest);

            GetStatusCode(result).Should().Be(500);
            saveRepairRequestUseCaseMock.Verify(x => x.Execute(repairRequest), Times.Once);
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
                Id = "1AB2C3D4",
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
            saveRepairRequestUseCaseMock.Setup(x => x.Execute(repairRequest)).ReturnsAsync(repair);

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
                    Type = "sms",
                    Value = "0765374057"
                },
                Time = new RepairAvailability
                {
                    Display = "Displayed Time"
                }
            };
            var repair = new Repair
            {
                Id = "1AB2C3D4",
                ContactDetails = new RepairContactDetails
                {
                    Type = "sms",
                    Value = "0765374057"
                },
                Time = new RepairAvailability
                {
                    Display = "Displayed Time"
                }
            };

            saveRepairRequestUseCaseMock.Setup(x => x.Execute(repairRequest)).ReturnsAsync(repair);

            //Act
            await systemUnderTest.SaveRepair(repairRequest);

            //Assert
            appointmentConfirmationSender.Verify(x => x.Execute(repair), Times.Once);
        }
    }
}
