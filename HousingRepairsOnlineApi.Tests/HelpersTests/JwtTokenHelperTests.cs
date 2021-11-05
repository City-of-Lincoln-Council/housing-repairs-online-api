using System;
using FluentAssertions;
using HousingRepairsOnlineApi.Helpers;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.HelpersTests
{
    public class JwtTokenHelperTests
    {
        private const string secret = "Secret";
        private const string issuer = "Issuer";
        private const string audience = "Audience";

        private JwtTokenHelper systemUnderTest = new(secret, issuer, audience);

        [Fact]
        public void WhenATokenIsGenerated_ThenANonNullTokenIsGenerated()
        {
            // Arrange

            // Act
            var actual = systemUnderTest.Generate();

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void WhenATokenIsGenerated_ThenANonEmptyTokenIsGenerated()
        {
            // Arrange

            // Act
            var actual = systemUnderTest.Generate();

            // Assert
            Assert.NotEmpty(actual);
        }

        [Fact]
        public void WhenATokenIsGenerated_ThenItIsUnexpired()
        {
            // Arrange
            //var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2MzYwOTc4ODEsImlzcyI6IkFuIElzc3VlciIsImF1ZCI6IlRoZSBBdWRpZW5jZSJ9.R9ck0EQAHuL4qbbnI1QUdV5nmYMnczkHr0-yhGmyNT8";

            // Act
            var actual = systemUnderTest.Generate();

            // Assert
            Action act = () => Decode(actual);

            act.Should().NotThrow<TokenExpiredException>();

            string Decode(string token)
            {
                return JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                    .WithSecret(secret)
                    .MustVerifySignature()
                    .Decode(token);
            }
        }
    }
}
