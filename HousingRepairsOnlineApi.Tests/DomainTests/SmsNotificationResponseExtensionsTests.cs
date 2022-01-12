using System.Collections.Generic;
using FluentAssertions;
using HousingRepairsOnlineApi.Domain;
using Moq;
using Notify.Models;
using Notify.Models.Responses;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.DomainTests
{
    public class SmsNotificationResponseExtensionsTests
    {
        [Fact]
        public void CanMapToSendSmsResponse()
        {
            //Arrange
            var personalisation = new Dictionary<string, dynamic>
            {
                {"booking_ref", "XXXX"},
                {"appointment_time", "10.00am"}
            };
            var result = new SmsNotificationResponse()
            {
                content = It.IsAny<SmsResponseContent>(),
                id = "id",
                reference = "reference",
                template = It.IsAny<Template>(),
                uri = It.IsAny<string>(),
            };

            //Act
            var response = result.ToSendSmsResponse("number", "templateId", personalisation);

            //Assert
            response.Should().BeOfType<SendSmsConfirmationResponse>();
            response.TemplateId.Should().Be("templateId");
            response.AppointmentTime.Should().Be(personalisation["appointment_time"]);
            response.BookingReference.Should().Be(personalisation["booking_ref"]);
            response.PhoneNumber.Should().Be("number");
            response.GovNotifyId.Should().Be("id");
        }
    }
}
