using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Gateways;
using Moq;
using Notify.Interfaces;
using Notify.Models.Responses;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.GatewaysTests
{
    public class GovNotifyGatewayTests
    {
        private readonly NotifyGateway systemUnderTest;
        private readonly Mock<INotificationClient> notifyClinet;

        public GovNotifyGatewayTests()
        {
            notifyClinet = new Mock<INotificationClient>();
            systemUnderTest = new NotifyGateway(notifyClinet.Object);
        }

        [Fact]
        public async Task GivenNoException_WhenSendSms_ThenSendSmsIsCalledOnClient()
        {
            //Arrange
            var personalisation = new Dictionary<string, dynamic>
            {
                {"booking_ref", "XXXX"},
                {"appointment_time", "10.00am"}

            };
            notifyClinet.Setup(x =>
                    x.SendSms(It.IsAny<string>(), It.IsAny<string>(), personalisation, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new SmsNotificationResponse { id = It.IsAny<string>() });

            //Act
            await systemUnderTest.SendSms("07415678534", "templateId", personalisation);

            //Assert
            notifyClinet.Verify(x => x.SendSms("07415678534", "templateId", personalisation, It.IsAny<string>(), It.IsAny<string>()), Times.Once());

        }

        [Fact]
        public async Task GivenNoException_WhenSendSms_ThenSendSmsResponseIsReturned()
        {
            //Arrange
            var personalisation = new Dictionary<string, dynamic>
            {
                {"booking_ref", "XXXX"},
                {"appointment_time", "10.00am"}

            };
            notifyClinet.Setup(x =>
                    x.SendSms(It.IsAny<string>(), It.IsAny<string>(), personalisation, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new SmsNotificationResponse { id = It.IsAny<string>() });

            //Act
            var result = await systemUnderTest.SendSms("07415678534", "templateId", personalisation);

            //Assert
            notifyClinet.Verify(x => x.SendSms("07415678534", "templateId", personalisation, It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            result.Should().BeOfType<SendSmsConfirmationResponse>();
        }

        [Fact]
        public async Task GivenNoException_WhenSendEmail_ThenSendEmailIsCalledOnClient()
        {
            //Arrange
            var personalisation = new Dictionary<string, dynamic>
            {
                {"booking_ref", "XXXX"},
                {"appointment_time", "10.00am"}

            };
            notifyClinet.Setup(x =>
                    x.SendEmail(It.IsAny<string>(), It.IsAny<string>(), personalisation, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new EmailNotificationResponse { id = It.IsAny<string>() });

            //Act
            await systemUnderTest.SendEmail("dr.who@tardis.com", "templateId", personalisation);

            //Assert
            notifyClinet.Verify(x => x.SendEmail("dr.who@tardis.com", "templateId", personalisation, It.IsAny<string>(), It.IsAny<string>()), Times.Once());

        }

        [Fact]
        public async Task GivenNoException_WhenSendEmail_ThenSendEmailResponseIsReturned()
        {
            //Arrange
            var personalisation = new Dictionary<string, dynamic>
            {
                {"booking_ref", "XXXX"},
                {"appointment_time", "10.00am"}

            };
            notifyClinet.Setup(x =>
                    x.SendEmail(It.IsAny<string>(), It.IsAny<string>(), personalisation, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new EmailNotificationResponse { id = It.IsAny<string>() });

            //Act
            var result = await systemUnderTest.SendEmail("dr.who@tardis.com", "templateId", personalisation);

            //Assert
            notifyClinet.Verify(x => x.SendEmail("dr.who@tardis.com", "templateId", personalisation, It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            result.Should().BeOfType<SendEmailConfirmationResponse>();
        }
    }
}
