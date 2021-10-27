using HousingRepairsOnlineApi.Helpers;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.HelpersTests
{
    public class SoREngineTests
    {
        [Theory]
        [InlineData("kitchen", "cupboards", "doorHangingOff", "N373049")]
        [InlineData("kitchen", "cupboards", "doorMissing", "N373049")]
        [InlineData("kitchen", "worktop", "damaged", "N372005")]
        [InlineData("kitchen", "dampMould", "dampMould", "N114001")]
        [InlineData("kitchen", "electrical", "extractorFan", "N841007")]
        [InlineData("kitchen", "electrical", "sockets", "N861511")]
        [InlineData("kitchen", "electrical", "lightFitting", "N858111")]
        //[InlineData("kitchen", "electrical", "cookerSwitch", "TBC-cookerSwitch")]
        [InlineData("kitchen", "sink", "taps", "N630147")]
        [InlineData("kitchen", "sink", "pipeworkLeak", "N630147")]
        //[InlineData("kitchen", "sink", "damage", "TBC-sinkDamage")]
        [InlineData("kitchen", "wallsFloorsCeiling", "wallTiles", "N431041")]
        [InlineData("kitchen", "wallsFloorsCeiling", "floorTiles", "N432005")]
        [InlineData("kitchen", "wallsFloorsCeiling", "lightFitting", "N858111")]
        [InlineData("kitchen", "wallsFloorsCeiling", "skirtingBoardArchitrave", "N381001")]
        [InlineData("kitchen", "wallsFloorsCeiling", "plasteringCeiling", "N413305")]
        [InlineData("kitchen", "wallsFloorsCeiling", "plasteringWalls", "N431023")]
        [InlineData("kitchen", "window", "stuckShut", "N318125")]
        [InlineData("kitchen", "window", "condensated", "N318151")]
        [InlineData("kitchen", "door", "backDoorWooden", "N324123")]
        [InlineData("kitchen", "door", "backDoorUPVC", "N325117")]
        [InlineData("kitchen", "door", "backFrenchDoors", "N325117")]
        [InlineData("kitchen", "door", "internal", "N330007")]
        [InlineData("kitchen", "door", "sliding", "N330007")]
        [InlineData("bathroom", "bath", "bathTaps", "N631301")]
        [InlineData("bathroom", "bath", "sealAroundBath", "N630945")]
        [InlineData("bathroom", "bath", "bathPanel", "N630945")]
        [InlineData("bathroom", "bath", "bathBlockage", "N630945")]
        [InlineData("bathroom", "shower", "electricShower", "N631131")]
        [InlineData("bathroom", "shower", "showerTap", "N631337")]
        [InlineData("bathroom", "shower", "showerHose", "N631111")]
        [InlineData("bathroom", "shower", "showerHead", "N631121")]
        [InlineData("bathroom", "sink", "sinkTaps", "N631301")]
        [InlineData("bathroom", "sink", "damagedSink", "N630714")]
        [InlineData("bathroom", "sink", "leakBlockedSink", "N620507")]
        [InlineData("bathroom", "toilet", "notFlushing", "N630573")]
        [InlineData("bathroom", "toilet", "overflowing", "N630573")]
        [InlineData("bathroom", "toilet", "looseFromFloor", "N630516")]
        public void GivenLocationProblemIssue_WhenCallingMapSorCode_ThenExpectedSorIsReturned(string location, string problem, string issue, string expectedSor)
        {
            // Arrange
            var sut = new SoREngine();

            // Act
            var actual = sut.MapSorCode(location, problem, issue);

            // Assert
            Assert.Equal(expectedSor, actual);
        }
    }
}
