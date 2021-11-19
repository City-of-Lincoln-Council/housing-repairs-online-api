using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HACT.Dtos;
using HousingRepairsOnlineApi.Gateways;
using Address = HousingRepairsOnlineApi.Domain.Address;

namespace HousingRepairsOnlineApi.UseCases
{
    public class RetrieveAddressesUseCase : IRetrieveAddressesUseCase
    {
        private readonly IAddressGateway addressGateway;

        public RetrieveAddressesUseCase(IAddressGateway addressGateway)
        {
            this.addressGateway = addressGateway;
        }

        public async Task<IEnumerable<Address>> Execute(string postcode)
        {
            if (postcode == null)
            {
                throw new ArgumentNullException(nameof(postcode));
            }
            var result = new List<Address>();
            if (!string.IsNullOrEmpty(postcode))
            {
                var addresses = await addressGateway.Search(postcode);
                result.AddRange(addresses.Select(ConvertToHactPropertyAddress));
            }

            return result;

            Address ConvertToHactPropertyAddress(PropertyAddress address)
            {
                var addressLine1Parts = new[] { address.BuildingNumber, address.AddressLine?.First() }
                    .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                var addressLine1 = addressLine1Parts.Any()
                    ? addressLine1Parts.Aggregate((s1, s2) => $"{s1} {s2}")
                    : null;

                return new Address
                {
                    Uprn = "",
                    AddressLine1 = addressLine1,
                    AddressLine2 = address.CityName,
                    PostCode = address.PostalCode
                };
            }
        }
    }
}
