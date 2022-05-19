using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Mappers;
using Xunit;
using FluentAssertions;
using System;

namespace HousingRepairsOnlineApi.Tests.MappersTests
{
    public class MapRepairsOnlineToRepairsHubTests
    {
        private readonly MapRepairsOnlineToRepairsHub sut;
        public MapRepairsOnlineToRepairsHubTests()
        {
            sut = new MapRepairsOnlineToRepairsHub();
        }

        [Fact]
        public async void DescriptionIsNullOrEmpty()
        {
            // Arrange
            var repairRequest = new RepairRequest
            {
                Description = new RepairDescriptionRequest
                {
                    Text = "Sample repair description"
                }
            };

            var repair = new Repair();

            // Act
            var result = sut.Map(repairRequest, repair);

            // Assert
            result.DescriptionOfWork.Should().NotBeNullOrEmpty();
        }
       

        // Reference can't' be null or empty and must be more than 20,000,000
        // Check that the priority, workclass, budget code, multi-trade are set
        // Instructed by is always "Hackney Housing" -> could get a new value for this?
        // Organisation always set to H01
        // AssignedToPrimary is always "HH General Building Repai"
        // Site has a single entry, has the address/postcode/propref
        // Work element is not null, has exactly one rate schedule item, has a trade
        // DateRaised and DateReported both set to today
        // Customer has a communication channel with one phone number
        // Customer's name has to be an empty string
    }
}
