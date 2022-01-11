using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.UseCasesTests
{
    public class SendAppointmentConfirmationSmsUseCaseTests
    {
        private readonly Mock<IGovNotifyGateway> govNotifyGatewayMock;
        private readonly SendAppointmentConfirmationSmsUseCase systemUnderTest;
        public SendAppointmentConfirmationSmsUseCaseTests()
        {
            govNotifyGatewayMock = new Mock<IGovNotifyGateway>();
            systemUnderTest = new SendAppointmentConfirmationSmsUseCase(govNotifyGatewayMock.Object, "templateId");
        }

        [Theory]
        [MemberData(nameof(InvalidNumberArgumentTestData))]
#pragma warning disable xUnit1026
        public async void GivenAnInvalidNumber_WhenExecute_ThenExceptionIsThrown<T>(T exception, string number) where T : Exception
#pragma warning restore xUnit1026
        {
            Func<Task> act = async () => await systemUnderTest.Execute(number, "bookingRef", "08:00am");

            await act.Should().ThrowExactlyAsync<T>();
        }

        public static IEnumerable<object[]> InvalidNumberArgumentTestData()
        {
            yield return new object[] { new ArgumentNullException(), null };
            yield return new object[] { new ArgumentException(), "" };
            yield return new object[] { new ArgumentException(), "0741630005444" };
        }

        [Theory]
        [MemberData(nameof(InvalidBookingRefArgumentTestData))]
#pragma warning disable xUnit1026
        public async void GivenAnInvalidBookingRef_WhenExecute_ThenExceptionIsThrown<T>(T exception, string bookingRef) where T : Exception
#pragma warning restore xUnit1026
        {
            Func<Task> act = async () => await systemUnderTest.Execute("number", bookingRef, "08:00am");

            await act.Should().ThrowExactlyAsync<T>();
        }
        public static IEnumerable<object[]> InvalidBookingRefArgumentTestData()
        {
            yield return new object[] { new ArgumentNullException(), null };
            yield return new object[] { new ArgumentException(), "" };
        }

        [Theory]
        [MemberData(nameof(InvalidAppointmentTimeArgumentTestData))]
#pragma warning disable xUnit1026
        public async void GivenAnInvalidAppointmentTime_WhenExecute_ThenExceptionIsThrown<T>(T exception, string appointmentTime) where T : Exception
#pragma warning restore xUnit1026
        {
            Func<Task> act = async () => await systemUnderTest.Execute("number", "bookingRef", appointmentTime);

            await act.Should().ThrowExactlyAsync<T>();
        }

        public static IEnumerable<object[]> InvalidAppointmentTimeArgumentTestData()
        {
            yield return new object[] { new ArgumentNullException(), null };
            yield return new object[] { new ArgumentException(), "" };
        }

        [Fact]
        public async void GivenValidParameters_WhenExecute_ThenGovNotifyGateWayIsCalled()
        {
            await systemUnderTest.Execute("07415300544", "bookingRef", "10:00am");
            govNotifyGatewayMock.Verify(x => x.SendSms("07415300544", "templateId", It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
        }
    }
}
