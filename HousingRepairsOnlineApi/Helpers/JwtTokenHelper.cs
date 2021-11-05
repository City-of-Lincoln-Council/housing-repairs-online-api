using System;
using JWT.Algorithms;
using JWT.Builder;

namespace HousingRepairsOnlineApi.Helpers
{
    public class JwtTokenHelper : IJwtTokenHelper
    {
        private readonly string secret;
        private string issuer;
        private string audience;

        public JwtTokenHelper(string secret, string issuer, string audience)
        {
            this.secret = secret;
            this.issuer = issuer;
            this.audience = audience;
        }

        public string Generate()
        {
            var result = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                .WithSecret(secret)
                .ExpirationTime(DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds())
                .Issuer(issuer)
                .Audience(audience)
                .Encode();

            return result;
        }
    }
}
