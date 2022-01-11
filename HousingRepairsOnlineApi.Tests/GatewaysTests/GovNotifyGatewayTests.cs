using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Notify.Interfaces;
using Notify.Models.Responses;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.GatewaysTests
{
    public class GovNotifyGatewayTests
    {
        private readonly GovNotifyGateway systemUnderTest;
        private readonly Mock<INotificationClient> notifyClinet;

        public GovNotifyGatewayTests()
        {
            notifyClinet = new Mock<INotificationClient>();
            systemUnderTest = new GovNotifyGateway(notifyClinet.Object);
        }

        [Fact]
        public async Task GivenNoException_WhenSendSms_ThenSendSmsIsCalledOnClient()
        {
            var personalisation = new Dictionary<string, dynamic>
            {
                {"booking_ref", "XXXX"},
                {"appointment_time", "10.00am"}

            };
            notifyClinet.Setup(x =>
                    x.SendSms(It.IsAny<string>(), It.IsAny<string>() , personalisation, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new SmsNotificationResponse{ id = It.IsAny<string>()});
            await systemUnderTest.SendSms("07415678534","templateId",  personalisation);
            notifyClinet.Verify(x => x.SendSms("07415678534","templateId",  personalisation, It.IsAny<string>(), It.IsAny<string>()), Times.Once());

        }
        [Fact]
        public async Task GivenNoException_WhenSendSms_ThenSendSmsResponseIsReturned()
        {

            var personalisation = new Dictionary<string, dynamic>
            {
                {"booking_ref", "XXXX"},
                {"appointment_time", "10.00am"}

            };
            notifyClinet.Setup(x =>
                    x.SendSms(It.IsAny<string>(), It.IsAny<string>() , personalisation, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new SmsNotificationResponse{ id = It.IsAny<string>()});
            var result = await systemUnderTest.SendSms("07415678534","templateId",  personalisation);
            notifyClinet.Verify(x => x.SendSms("07415678534","templateId",  personalisation, It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            result.Should().BeOfType<SendSmsResponse>();
        }
    }
}
