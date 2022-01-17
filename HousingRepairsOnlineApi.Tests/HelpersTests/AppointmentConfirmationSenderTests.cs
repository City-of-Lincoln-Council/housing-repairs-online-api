﻿using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.HelpersTests
{
    public class AppointmentConfirmationSenderTests
    {
        private readonly Mock<ISendAppointmentConfirmationEmailUseCase> sendAppointmentConfirmationEmailUseCaseMock;
        private readonly Mock<ISendAppointmentConfirmationSmsUseCase> sendAppointmentConfirmationSmsUseCaseMock;
        private readonly AppointmentConfirmationSender systemUnderTest;

        public AppointmentConfirmationSenderTests()
        {
            sendAppointmentConfirmationEmailUseCaseMock = new Mock<ISendAppointmentConfirmationEmailUseCase>();
            sendAppointmentConfirmationSmsUseCaseMock = new Mock<ISendAppointmentConfirmationSmsUseCase>();
            systemUnderTest = new AppointmentConfirmationSender(sendAppointmentConfirmationEmailUseCaseMock.Object, sendAppointmentConfirmationSmsUseCaseMock.Object);
        }

        [Fact]
        public void GivenEmailContact_WhenExecute_ThenSendAppointmentConfirmationEmailUseCaseIsCalled()
        {
            var repairRequest = new RepairRequest { ContactDetails = new RepairContactDetails { Type = HousingRepairsOnlineApi.Helpers.AppointmentConfirmationSendingTypes.Email, Value = "abc@defg.hij" }, Time = new RepairAvailability() { Display = "some time" } };
            systemUnderTest.Execute(repairRequest, "id");
            sendAppointmentConfirmationEmailUseCaseMock.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            sendAppointmentConfirmationSmsUseCaseMock.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void GivenSmsContact_WhenExecute_ThenSendAppointmentConfirmationSmsUseCaseIsCalled()
        {
            var repairRequest = new RepairRequest { ContactDetails = new RepairContactDetails { Type = HousingRepairsOnlineApi.Helpers.AppointmentConfirmationSendingTypes.Sms, Value = "0754325678" }, Time = new RepairAvailability() { Display = "some time" } };
            systemUnderTest.Execute(repairRequest, "id");
            sendAppointmentConfirmationEmailUseCaseMock.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            sendAppointmentConfirmationSmsUseCaseMock.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
