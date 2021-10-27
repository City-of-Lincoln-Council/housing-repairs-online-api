using System.Collections.Generic;
using HousingRepairsOnlineApi.Helpers;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.HelpersTests
{
    public class SoREngineTests
    {
        private SoREngine systemUnderTest;

        public SoREngineTests()
        {
            IDictionary<string, IDictionary<string, IDictionary<string, string>>> mapping = new Dictionary<string, IDictionary<string, IDictionary<string, string>>>()
            {
                {
                    "kitchen",
                    new Dictionary<string, IDictionary<string, string>>
                    {
                        {
                            "cupboards", new Dictionary<string, string>
                            {
                                { "doorHangingOff", "N373049" },
                                { "doorMissing", "N373049" },
                            }
                        },
                        {
                            "worktop", new Dictionary<string, string>
                            {
                                { "damaged", "N372005" },
                            }
                        },
                    }
                },
                {
                    "bathroom",
                    new Dictionary<string, IDictionary<string, string>>
                    {
                        {
                            "bath", new Dictionary<string, string>
                            {
                                {"bathTaps", "N631301"},

                            }
                        },
                    }
                }
            };
            systemUnderTest = new SoREngine(mapping);
        }

        [Theory]
        [InlineData("kitchen", "cupboards", "doorHangingOff", "N373049")]
        [InlineData("kitchen", "cupboards", "doorMissing", "N373049")]
        [InlineData("kitchen", "worktop", "damaged", "N372005")]
        [InlineData("bathroom", "bath", "bathTaps", "N631301")]
        public void GivenLocationProblemIssue_WhenCallingMapSorCode_ThenExpectedSorIsReturned(string location, string problem, string issue, string expectedSor)
        {
            // Arrange

            // Act
            var actual = systemUnderTest.MapSorCode(location, problem, issue);

            // Assert
            Assert.Equal(expectedSor, actual);
        }
    }
}
