using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.Mappers;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.MappersTests
{
    public class MapRepairsOnlineToRepairsHubTests
    {
        private readonly MapRepairsOnlineToRepairsHub sut;
        private string validRepairRequestPostcode = "N4 2FL";
        private string validRepairRequestPropertyyReference = "00003277";

        public MapRepairsOnlineToRepairsHubTests()
        {
            var sorEngineMock = new Mock<ISoREngine>();
            sut = new MapRepairsOnlineToRepairsHub(sorEngineMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void DescriptionIsNotNullOrEmpty(string desc)
        {
            // Arrange
            var repairRequest = new RepairRequest
            {
                Description = new RepairDescriptionRequest
                {
                    Text = desc
                }
            };

            var repair = GenerateValidRepair();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => sut.Map(repairRequest));
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("55000")]
        [InlineData("19999999")]
        [InlineData("30000001")]
        public void ReferenceIsNotEmptyAndIsGreaterThan20Mil(string id)
        {
            // Arrange
            var repairRequest = GenerateValidRequest();

            var repair = GenerateValidRepair();
            repair.Id = id;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Map(repairRequest));
        }

        [Fact]
        public void ReferenceIsNotNull()
        {
            // Arrange
            var repairRequest = GenerateValidRequest();

            var repair = new Repair
            {
                Id = null
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.Map(repairRequest));
        }

        [Fact]
        public void PriorityIsSetCorrectly()
        {
            // Arrange
            var repairRequest = GenerateValidRequest();
            var repair = GenerateValidRepair();

            // Act
            var result = sut.Map(repairRequest);

            // Assert
            result.Priority.PriorityCode.Should().Be(4);
            result.Priority.PriorityDescription.Should().Be("5 [N] NORMAL");
            result.Priority.NumberOfDays.Should().Be(21);
        }

        [Fact]
        public void FixedAttributesSetCorrectly()
        {
            // Arrange
            var repairRequest = GenerateValidRequest();
            var repair = GenerateValidRepair();

            // Act
            var result = sut.Map(repairRequest);

            // Assert
            result.WorkClass.WorkClassCode.Should().Be(0);
            result.BudgetCode.Id.Should().Be("8");
            result.MultiTradeWorkOrder.Should().BeFalse();
        }

        [Fact]
        public void OrganisationSetCorrectly()
        {
            // Arrange
            var repairRequest = GenerateValidRequest();
            var repair = GenerateValidRepair();

            // Act
            var result = sut.Map(repairRequest);

            // Assert
            result.InstructedBy.Name.Should().Be("Hackney Housing");
            result.AssignedToPrimary.Name.Should().Be("HH General Building Repai");
        }

        [Fact]
        public void AddressDataIsSetCorrectly()
        {
            // Arrange
            var repairRequest = GenerateValidRequest();
            var repair = GenerateValidRepair();

            // Act
            var result = sut.Map(repairRequest);

            // Assert
            var prop = result.Site.Property.Single();

            prop.PropertyReference.Should().Be(validRepairRequestPropertyyReference);
            prop.Address.PostalCode.Should().Be(validRepairRequestPostcode);
        }

        [Fact]
        public void CustomerNameIsBlank()
        {
            // Arrange
            var repairRequest = GenerateValidRequest();
            var repair = GenerateValidRepair();

            // Act
            var result = sut.Map(repairRequest);

            // Assert
            result.Customer.Name.Should().Be("");
            result.Customer.Person.Name.Full.Should().Be("");
        }

        [Fact]
        public void CustomerPhoneIsCorrect()
        {
            // Arrange
            var repairRequest = GenerateValidRequest();
            var repair = GenerateValidRepair();

            // Act
            var result = sut.Map(repairRequest);

            // Assert
            var comm = result.Customer.Person.Communication.Single();
            comm.Channel.Medium.Should().Be("20");
            comm.Channel.Code.Should().Be("60");
            comm.Value.Should().Be("07720340340");
        }

        [Fact]
        public void WorkElementIsCorrect()
        {
            // Arrange
            var repairRequest = GenerateValidRequest();
            var repair = GenerateValidRepair();

            // Act
            var result = sut.Map(repairRequest);

            // Assert
            var workElement = result.WorkElement.Single();

            var sor = workElement.RateScheduleItem.Single();
            sor.CustomCode.Should().Be("20110200");
            sor.CustomName.Should().Be("TBC");
        }

        private RepairRequest GenerateValidRequest()
        {
            return new RepairRequest
            {
                Description = new RepairDescriptionRequest
                {
                    Text = "Valid description"
                },
                Address = new RepairAddress
                {
                    LocationId = validRepairRequestPropertyyReference,
                    Display = "Sample location 1"
                },
                Postcode = validRepairRequestPostcode,
                ContactPersonNumber = "07720340340"
            };
        }

        private Repair GenerateValidRepair()
        {
            return new Repair
            {
                Id = "20000000",
                SOR = "20110200"
            };
        }

        // Work element is not null, has exactly one rate schedule item, has a trade
    }
}
