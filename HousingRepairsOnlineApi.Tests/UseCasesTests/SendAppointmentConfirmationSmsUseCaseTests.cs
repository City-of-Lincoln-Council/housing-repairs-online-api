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
        private readonly Mock<INotifyGateway> govNotifyGatewayMock;
        private readonly SendAppointmentConfirmationSmsUseCase systemUnderTest;
        public SendAppointmentConfirmationSmsUseCaseTests()
        {
            govNotifyGatewayMock = new Mock<INotifyGateway>();
            systemUnderTest = new SendAppointmentConfirmationSmsUseCase(govNotifyGatewayMock.Object, "templateId");
        }

        //Arrange
        public static IEnumerable<object[]> InvalidNumberArgumentTestData()
        {
            yield return new object[] { new ArgumentNullException(), null };
            yield return new object[] { new ArgumentException(), "" };
            yield return new object[] { new ArgumentException(), "074353000554" };
        }

        [Theory]
        [MemberData(nameof(InvalidNumberArgumentTestData))]
#pragma warning disable xUnit1026
        public async void GivenAnInvalidNumber_WhenExecute_ThenExceptionIsThrown<T>(T exception, string number) where T : Exception
#pragma warning restore xUnit1026
        {
            //Act
            Func<Task> act = async () => systemUnderTest.Execute(number, "bookingRef", "08:00am");

            //Assert
            await act.Should().ThrowExactlyAsync<T>();
        }

        //Arrange
        public static IEnumerable<object[]> InvalidBookingRefArgumentTestData()
        {
            yield return new object[] { new ArgumentNullException(), null };
            yield return new object[] { new ArgumentException(), "" };
        }

        [Theory]
        [MemberData(nameof(InvalidBookingRefArgumentTestData))]
#pragma warning disable xUnit1026
        public async void GivenAnInvalidBookingRef_WhenExecute_ThenExceptionIsThrown<T>(T exception, string bookingRef) where T : Exception
#pragma warning restore xUnit1026
        {
            //Act
            Func<Task> act = async () => systemUnderTest.Execute("number", bookingRef, "08:00am");

            //Assert
            await act.Should().ThrowExactlyAsync<T>();
        }

        //Arrange
        public static IEnumerable<object[]> InvalidAppointmentTimeArgumentTestData()
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
            //Act
            Func<Task> act = async () => systemUnderTest.Execute("number", "bookingRef", appointmentTime);

            //Assert
            await act.Should().ThrowExactlyAsync<T>();
        }

        [Fact]
        public async void GivenValidParameters_WhenExecute_ThenGovNotifyGateWayIsCalled()
        {
            //Act
            systemUnderTest.Execute("07415300544", "bookingRef", "10:00am");

            //Assert
            govNotifyGatewayMock.Verify(x => x.SendSms("07415300544", "templateId", It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
        }
    }
}
