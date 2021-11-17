using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HACT.Dtos;
using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.UseCasesTests
{
    public class RetrieveAddressesUseCaseTests
    {
        private readonly RetrieveAddressesUseCase sytemUndertest;
        private readonly Mock<IAddressGateway> addressGatewayMock;

        public RetrieveAddressesUseCaseTests()
        {
            addressGatewayMock = new Mock<IAddressGateway>();
            sytemUndertest = new RetrieveAddressesUseCase(addressGatewayMock.Object);
        }

        [Fact]
        public async Task ReturnsEmptyWhenNoAddressesAreFound()
        {
            var data = await sytemUndertest.Execute("");
            data.Should().BeEmpty();
        }

        [Fact]
        public void GatewayGetsCalledWithPostCode()
        {
            const string TestPostcode = "M3 0W";
            sytemUndertest.Execute(postcode: TestPostcode);
            addressGatewayMock.Verify(x => x.Search(TestPostcode), Times.Once);
        }

        [Fact]
        public async Task DoesNotReturnAnEmptyCollectionOfAddresses()
        {
            const string TestPostcode = "M3 0W";
            addressGatewayMock.Setup(x => x.Search(It.IsAny<string>()))
                .ReturnsAsync(new List<PropertyAddress>());
            var data = await sytemUndertest.Execute(postcode: TestPostcode);
            Assert.Empty(data);
        }

        [Fact]
        public async Task ReturnACollectionOfAddresses()
        {
            const string TestPostcode = "M3 0W";
            var testAddress = new PropertyAddress()
            {
                AddressLine = new Collection<string>() { "123 cute street" },
                PostalCode = TestPostcode,
                CityName = "New Meow City"
            };
            addressGatewayMock.Setup(x => x.Search(It.IsAny<string>()))
                .ReturnsAsync(new List<PropertyAddress>() { testAddress });
            var data = await sytemUndertest.Execute(postcode: TestPostcode);
            Assert.Equal(data.First().AddressLine1, testAddress.AddressLine.First());
            Assert.Equal(data.First().AddressLine2, testAddress.CityName);
            Assert.Equal(data.First().PostCode, TestPostcode);
        }

        [Fact]
        public async void ThrowsNullExceptionWhenPostcodeIsNull()
        {
            const string TestPostcode = null;
            Func<Task> act = async () => await sytemUndertest.Execute(TestPostcode);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public void DoesNotCallAddressGatewayWhenPostcodeIsEmpty()
        {
            const string TestPostcode = "";
            var data = sytemUndertest.Execute(postcode: TestPostcode);
            addressGatewayMock.Verify(x => x.Search(TestPostcode), Times.Never);
        }

        [Fact]
        public async void GivenAnAddressWithoutAnAddressLine_WhenExecuteIsCalled_ThenAnExceptionIsNotThrown()
        {
            // Arrange
            const string postCode = "LN1 3PQ";
            addressGatewayMock.Setup(x => x.Search(postCode))
                .ReturnsAsync(new List<PropertyAddress> { new PropertyAddress { PostalCode = postCode } });

            // Act
            Func<Task> act = async () => await sytemUndertest.Execute(postCode);

            // Assert
            await act.Should().NotThrowAsync();
        }
    }
}
