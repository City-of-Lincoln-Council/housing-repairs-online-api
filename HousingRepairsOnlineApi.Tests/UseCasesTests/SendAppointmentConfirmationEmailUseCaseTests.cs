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
    public class SendAppointmentConfirmationEmailUseCaseTests
    {
        private readonly Mock<INotifyGateway> govNotifyGatewayMock;
        private readonly SendAppointmentConfirmationEmailUseCase systemUnderTest;

        public SendAppointmentConfirmationEmailUseCaseTests()
        {
            govNotifyGatewayMock = new Mock<INotifyGateway>();
            systemUnderTest = new SendAppointmentConfirmationEmailUseCase(govNotifyGatewayMock.Object, "templateId");
        }

        //Arrange
        public static IEnumerable<object[]> InvalidEmailArgumentTestData()
        {
            yield return new object[] { new ArgumentNullException(), null };
            yield return new object[] { new ArgumentException(), "" };
            yield return new object[] { new ArgumentException(), "notanemail.com" };
        }

        [Theory]
        [MemberData(nameof(InvalidEmailArgumentTestData))]
#pragma warning disable xUnit1026
        public async void GivenAnInvalidEmail_WhenExecute_ThenExceptionIsThrown<T>(T exception, string email) where T : Exception
#pragma warning restore xUnit1026
        {
            //Act
            Func<Task> act = async () => systemUnderTest.Execute(email, "bookingRef", "08:00am");

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
            Func<Task> act = async () => systemUnderTest.Execute("dr.who@tardis.com", bookingRef, "08:00am");

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
            Func<Task> act = async () => systemUnderTest.Execute("dr.who@tardis.com", "bookingRef", appointmentTime);

            //Assert
            await act.Should().ThrowExactlyAsync<T>();
        }

        [Fact]
        public async void GivenValidParameters_WhenExecute_ThenGovNotifyGateWayIsCalled()
        {
            //Act
            systemUnderTest.Execute("dr.who@tardis.com", "bookingRef", "10:00am");

            //Assert
            govNotifyGatewayMock.Verify(x => x.SendEmail("dr.who@tardis.com", "templateId", It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
        }
    }
}
