﻿using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Controllers;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests
{
    public class RepairRequestsControllerTests : ControllerTests
    {
        private RepairController systemUnderTest;
        private Mock<ISaveRepairRequestUseCase> saveRepairRequestUseCaseMock;
        private Mock<ISendAppointmentConfirmationEmailUseCase> sendAppointmentConfirmationEmailUseCase;
        private Mock<ISendAppointmentConfirmationSmsUseCase> sendAppointmentConfirmationSmsUseCase;

        public RepairRequestsControllerTests()
        {
            saveRepairRequestUseCaseMock = new Mock<ISaveRepairRequestUseCase>();
            sendAppointmentConfirmationEmailUseCase = new Mock<ISendAppointmentConfirmationEmailUseCase>();
            sendAppointmentConfirmationSmsUseCase = new Mock<ISendAppointmentConfirmationSmsUseCase>();
            systemUnderTest = new RepairController(saveRepairRequestUseCaseMock.Object, sendAppointmentConfirmationEmailUseCase.Object, sendAppointmentConfirmationSmsUseCase.Object);
        }

        [Fact]
        public async Task TestEndpoint()
        {
            RepairRequest repairRequest = new RepairRequest();
            const string RepairId = "1AB2C3D4";
            saveRepairRequestUseCaseMock.Setup(x => x.Execute(It.IsAny<RepairRequest>())).ReturnsAsync(RepairId);

            var result = await systemUnderTest.SaveRepair(repairRequest);

            GetStatusCode(result).Should().Be(200);
            saveRepairRequestUseCaseMock.Verify(x => x.Execute(repairRequest), Times.Once);
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
            const string RepairId = "1AB2C3D4";
            saveRepairRequestUseCaseMock.Setup(x => x.Execute(repairRequest)).ReturnsAsync(RepairId);

            //Assert
            await systemUnderTest.SaveRepair(repairRequest);

            //Act
            sendAppointmentConfirmationEmailUseCase.Verify(x => x.Execute(repairRequest.ContactDetails.Value, RepairId, "Displayed Time"), Times.Once);
        }

        [Fact]
        public async Task GivenSmsContact_WhenRepair_ThenSendAppointmentConfirmationSmsUseCaseIsCalled()
        {
            //Arrange
            RepairRequest repairRequest = new RepairRequest
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
            const string RepairId = "1AB2C3D4";
            saveRepairRequestUseCaseMock.Setup(x => x.Execute(repairRequest)).ReturnsAsync(RepairId);

            //Act
            await systemUnderTest.SaveRepair(repairRequest);

            //Assert
            sendAppointmentConfirmationSmsUseCase.Verify(x => x.Execute(repairRequest.ContactDetails.Value, RepairId, "Displayed Time"), Times.Once);
        }
    }
}
