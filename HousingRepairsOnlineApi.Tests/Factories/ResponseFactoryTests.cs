using System.Collections.Generic;
using FluentAssertions;
using HousingRepairsOnlineApi.Factories;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Notify.Models;
using Notify.Models.Responses;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.Factories
{
    public class ResponseFactoryTests
    {
        [Fact]
        public void CanMapToSendSmsResponse()
        {
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
                uri = It.IsAny<string>()
            };
            var response = result.ToResponse("number", "templateId", personalisation);
            response.Should().BeOfType<SendSmsResponse>();
        }
    }
}
