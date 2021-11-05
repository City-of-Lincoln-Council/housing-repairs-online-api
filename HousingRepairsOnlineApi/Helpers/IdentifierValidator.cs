using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace HousingRepairsOnlineApi.Helpers
{
    public class IdentifierValidator : IIdentifierValidator
    {
        private readonly string identifier;

        public IdentifierValidator([NotNull] string identifier)
        {
            Guard.Against.NullOrWhiteSpace(identifier, nameof(identifier));

            this.identifier = identifier;
        }

        public bool Validate(string identifier)
        {
            return identifier == this.identifier;
        }
    }
}
