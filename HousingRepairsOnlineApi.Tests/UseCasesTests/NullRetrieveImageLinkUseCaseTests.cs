using HousingRepairsOnlineApi.UseCases;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.UseCasesTests
{
    public class NullRetrieveImageLinkUseCaseTests
    {
        private readonly NullRetrieveImageLinkUseCase systemUnderTest = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("Z")]
        [InlineData("~")]
        [InlineData("@")]
        public void ReturnsInputParameter(string input)
        {
            // Arrange

            // Act
            var actual = systemUnderTest.Execute(input);

            // Assert
            Assert.Equal(input, actual);
        }
    }
}
