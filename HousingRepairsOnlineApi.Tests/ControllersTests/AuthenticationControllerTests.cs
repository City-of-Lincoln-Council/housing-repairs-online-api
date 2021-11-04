using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using HousingRepairsOnlineApi.Controllers;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.ControllersTests
{
    public class AuthenticationControllerTests : ControllerTests
    {
        private AuthenticationController systemUnderTest;

        public AuthenticationControllerTests()
        {
            systemUnderTest = new AuthenticationController(null);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task GivenAnInvalidIdentifier_WhenAuthenticateIsCalled_ThenUnauthorisedResponseIsGiven(string identifier)
        {
            var result = await systemUnderTest.Authenticate(identifier);

            GetStatusCode(result).Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GivenAValidIdentifierThatIsNotAuthorised_WhenAuthenticateIsCalled_ThenUnauthorisedResponseIsGiven()
        {
            const string identifier = "M3";
            var result = await systemUnderTest.Authenticate(identifier);

            GetStatusCode(result).Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GivenAValidIdentifierThatIsAuthorised_WhenAuthenticateIsCalled_ThenOkResponseIsGiven()
        {
            const string identifier = "M3";
            systemUnderTest = new AuthenticationController(identifier);
            var result = await systemUnderTest.Authenticate(identifier);

            GetStatusCode(result).Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenAValidIdentifierThatIsAuthorised_WhenAuthenticateIsCalled_ThenResponseContainsANonNullAndNonEmptyString()
        {
            const string identifier = "M3";
            systemUnderTest = new AuthenticationController(identifier);
            var result = await systemUnderTest.Authenticate(identifier);

            GetResultData<string>(result).Should().NotBeNull().And.NotBeEmpty();
        }
    }
}
