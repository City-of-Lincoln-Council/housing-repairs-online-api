using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Controllers;
using HousingRepairsOnlineApi.Helpers;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.ControllersTests
{
    public class AuthenticationControllerTests : ControllerTests
    {
        private AuthenticationController systemUnderTest;
        private Mock<IIdentifierValidator> identifierValidatorMock;
        private Mock<IJwtTokenHelper> jwtTokenHelperMock;

        public AuthenticationControllerTests()
        {
            identifierValidatorMock = new Mock<IIdentifierValidator>();
            jwtTokenHelperMock = new Mock<IJwtTokenHelper>();
            systemUnderTest = new AuthenticationController(identifierValidatorMock.Object, jwtTokenHelperMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task GivenAnInvalidIdentifier_WhenAuthenticateIsCalled_ThenUnauthorisedResponseIsGiven(string identifier)
        {
            // Arrange
            identifierValidatorMock.Setup(x => x.Validate(It.IsAny<string>())).Returns(false);

            // Act
            var result = await systemUnderTest.Authenticate(identifier);

            // Assert
            GetStatusCode(result).Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GivenAValidIdentifierThatIsNotAuthorised_WhenAuthenticateIsCalled_ThenUnauthorisedResponseIsGiven()
        {
            // Arrange
            const string identifier = "M3";
            identifierValidatorMock.Setup(x => x.Validate(identifier)).Returns(false);

            // Act
            var result = await systemUnderTest.Authenticate(identifier);

            // Assert
            GetStatusCode(result).Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GivenAValidIdentifierThatIsAuthorised_WhenAuthenticateIsCalled_ThenOkResponseIsGiven()
        {
            // Arrange
            const string identifier = "M3";
            identifierValidatorMock.Setup(x => x.Validate(identifier)).Returns(true);

            // Act
            var result = await systemUnderTest.Authenticate(identifier);

            // Assert
            GetStatusCode(result).Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenAValidIdentifierThatIsAuthorised_WhenAuthenticateIsCalled_ThenResponseContainsANonNullAndNonEmptyString()
        {
            // Arrange
            const string identifier = "M3";
            identifierValidatorMock.Setup(x => x.Validate(identifier)).Returns(true);
            jwtTokenHelperMock.Setup(x => x.Generate()).Returns("a token");

            // Act
            var result = await systemUnderTest.Authenticate(identifier);

            // Assert
            GetResultData<string>(result).Should().NotBeNull().And.NotBeEmpty();
        }
    }
}
